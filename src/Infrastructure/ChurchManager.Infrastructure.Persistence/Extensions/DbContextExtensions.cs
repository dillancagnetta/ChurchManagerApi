using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Extensions;

public static class DbContextExtensions
{
    /*
     context.AddManyToMany<StudentCourse, Student, Course>(
        context.StudentCourses,
        firstId: 1,
        secondId: 2,
        configure: sc => sc.EnrolledDate = DateTime.UtcNow
    );
    context.SaveChanges();

     */
    public static void AddManyToMany<TJoin, T1, T2>(
        this DbContext context,
        DbSet<TJoin> dbSet,
        int firstId,
        int secondId,
        Action<TJoin> configure = null)
        where TJoin : class, new()
    {
        var joinEntity = new TJoin();
        typeof(TJoin).GetProperty(typeof(T1).Name + "Id")?.SetValue(joinEntity, firstId);
        typeof(TJoin).GetProperty(typeof(T2).Name + "Id")?.SetValue(joinEntity, secondId);
        configure?.Invoke(joinEntity);
        dbSet.Add(joinEntity);
    }

    /*
     context.RemoveManyToMany<StudentCourse>(context.StudentCourses, firstId: 1, secondId: 2);
     context.SaveChanges();
     */
    public static void RemoveManyToMany<TJoin>(
        this DbContext context,
        DbSet<TJoin> dbSet,
        int firstId,
        int secondId)
        where TJoin : class
    {
        var joinEntity = dbSet.Find(firstId, secondId);
        if (joinEntity != null)
        {
            dbSet.Remove(joinEntity);
        }
    }

    /*
     context.UpdateManyToMany<StudentCourse>(
        context.StudentCourses,
        firstId: 1,
        secondId: 2,
        update: sc => sc.EnrolledDate = DateTime.UtcNow.AddDays(10)
    );
    context.SaveChanges();
     */
    public static void UpdateManyToMany<TJoin>(
        this DbContext context,
        DbSet<TJoin> dbSet,
        int firstId,
        int secondId,
        Action<TJoin> update)
        where TJoin : class
    {
        var joinEntity = dbSet.Find(firstId, secondId);
        if (joinEntity != null)
        {
            update(joinEntity);
            context.Update(joinEntity);
        }
    }
    
    /*
    batch update method for explicit many-to-many relationships. It will:
        Add new relationships if they don’t exist.
        Remove relationships that are missing from the new list.
        --------------------------------------------------------
        
         context.UpdateManyToMany<StudentCourse>(
            context.StudentCourses,
            firstId: 1, // StudentId
            newSecondIds: new List<int> { 2, 3, 4 }, // New CourseIds
            getFirstId: sc => sc.StudentId,
            getSecondId: sc => sc.CourseId,
            createJoinEntity: (studentId, courseId) => new StudentCourse { StudentId = studentId, CourseId = courseId }
        );

        context.SaveChanges();

     */
    public static void UpdateManyToMany<TJoin>(
        this DbContext context,
        DbSet<TJoin> dbSet,
        int firstId,
        IEnumerable<int> newSecondIds,
        Func<TJoin, int> getFirstId,
        Func<TJoin, int> getSecondId,
        Func<int, int, TJoin> createJoinEntity)
        where TJoin : class
    {
        // Load existing relationships into memory
        var existingRelations = dbSet.AsNoTracking().Where(j => getFirstId(j) == firstId).ToList();

        var existingIds = existingRelations.Select(getSecondId).ToHashSet();
        var newIds = newSecondIds.ToHashSet();

        // Identify items to add
        var toAdd = newIds.Except(existingIds);
        foreach (var secondId in toAdd)
        {
            dbSet.Add(createJoinEntity(firstId, secondId));
        }

        // Identify items to remove
        var toRemove = existingRelations.Where(r => !newIds.Contains(getSecondId(r))).ToList();
        dbSet.RemoveRange(toRemove);
    }
}