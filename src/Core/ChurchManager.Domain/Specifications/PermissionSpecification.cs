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

public class PermissionCriteria<T> where T : class, Codeboss.Types.IEntity<int>
{
    public static void AddPermissionCriteria (
        ISpecificationBuilder<T> query,
        Guid userId,
        IPermissionService service,
        string permission = "View") 
    {
        query.Where(BuildPermissionExpression(userId, service, permission));
    }
    
    // defer the permission check to when the query is executed
    private static Expression<Func<T, bool>> BuildPermissionExpression(Guid userId, IPermissionService service, string permission = "View")
    {
        return entity => service.HasPermissionAsync<T>(userId, entity.Id, permission, new CancellationToken()).GetAwaiter().GetResult();
    }
}