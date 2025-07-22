using System.Linq.Expressions;
using ChurchManager.Domain.Features.Finances;
using ChurchManager.Domain.Features.Finances.Extensions;
using ChurchManager.Domain.Features.Finances.Repositories;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Repositories;

public class FundsDbRepository: GenericRepositoryBase<Fund>, IFundsDbRepository
{
    private readonly ChurchManagerDbContext _dbContext;

    public FundsDbRepository(ChurchManagerDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<FundViewModel>> FundsWithChildrenFlatAsync(bool onlyShowInNavigation = true, CancellationToken ct = default)
    {
        // Gets the root group and all its descendants in a flattened list
        var filter = !onlyShowInNavigation ? "" : "WHERE f.\"ShowInNavigation\" = true";
        var query = @$"
                    WITH RECURSIVE RecursiveGroups AS (
                        SELECT * FROM ""Finances"".""Fund"" f {filter}
                        UNION
                        SELECT g.*
                        FROM ""Finances"".""Fund"" g INNER JOIN RecursiveGroups rg ON g.""ParentFundId"" = rg.""Id""
                    )
                    SELECT * FROM RecursiveGroups";
        
        return await _dbContext.Fund.FromSqlRaw(query) 
            .Select(g => g.ToModel()!)
            .ToListAsync(ct);
    }
    
    /// <summary>
    /// https://michaelceber.medium.com/implementing-a-recursive-projection-query-in-c-and-entity-framework-core-240945122be6
    /// </summary>
    private Expression<Func<Fund, FundViewModel>> ChildProjection(int maxDepth, int currentDepth = 0)
    {
        currentDepth++;

        Expression<Func<Fund, FundViewModel>> result = fund => new FundViewModel
        {
            Id = fund.Id,
            Name = fund.Name,
            Description = fund.Description,
            Code = fund.Code,
            FundType = fund.FundType.Value,
            ParentFundId = fund.ParentFundId,
            Funds = currentDepth == maxDepth
                ? new List<FundViewModel>(0) // Reached maximum depth so stop
                : fund.ChildFunds.AsQueryable()
                    .Select(ChildProjection(maxDepth, currentDepth))
                    .ToList()
        };

        return result;
    }
}