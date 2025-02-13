using System.Linq.Expressions;
using Ardalis.Specification;
using ChurchManager.Domain.Features.Permissions.Services;

namespace ChurchManager.Domain.Specifications;

public class PermissionSpecification<T> : Specification<T> where T : class, Codeboss.Types.IEntity<int>
{
    private readonly IPermissionService _service;
    private readonly Guid _userId;
    private readonly string _permission;

    public PermissionSpecification(
        IPermissionService service,
        Guid userId,
        string permission = "View")
    {
        _service = service;
        _userId = userId;
        _permission = permission;
        
        // Apply the permission filter using the Query property
        Query.Where(BuildPermissionExpression());
    }
    
    // defer the permission check to when the query is executed
    private Expression<Func<T, bool>> BuildPermissionExpression()
    {
        return entity => _service.HasPermissionAsync<T>(_userId, entity.Id, _permission, new CancellationToken()).GetAwaiter().GetResult();
    }
}

public class PermissionSpecification<T, V> : Specification<T, V> where T : class, Codeboss.Types.IEntity<int>
{
    protected PermissionSpecification(IEnumerable<int> allowedChurchIds = null)
    {
        // Only apply permission filter if allowedIds is not null
        // If null, user is system admin and has unrestricted access
        if (allowedChurchIds is not null)
        {
            // First apply the permissions filter
            Query.Where(x => allowedChurchIds.Contains(x.Id));  
        }
    }
    
}

public class PermissionCriteria<T> where T : class, Codeboss.Types.IEntity<int>
{
    public static async Task AddPermissionCriteria (
        ISpecificationBuilder<T> query,
        Guid userId,
        IPermissionService service,
        string permission = "View") 
    {
        // Get all allowed entity IDs asynchronously
        var allowedIds = await service.GetAllowedEntityIdsAsync<T>(userId, permission);
        
        // Use Task.Result to get the result synchronously
        // Note: This is not ideal in terms of async/await best practices, but it allows us to use the result in the LINQ query
        //var allowedIds = await allowedIdsTask;
        
        // Add the criteria to the query
        query.Where(e => allowedIds.Contains(e.Id));
    }
    
    // defer the permission check to when the query is executed
    private static Expression<Func<T, bool>> BuildPermissionExpression(Guid userId, IPermissionService service, string permission = "View")
    {
        return entity => service.HasPermissionAsync<T>(userId, entity.Id, permission, new CancellationToken()).GetAwaiter().GetResult();
    }
}