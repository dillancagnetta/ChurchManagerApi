using ChurchManager.Domain.Features.Security.Services;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using ChurchManager.Infrastructure.Abstractions.Persistence;

namespace ChurchManager.Features.Auth.Services;

public class EntityPermissionsResolver(
    ChurchManagerDbContext dbContext,
    IQueryCache cache) : IEntityPermissionsResolver
{
    public async Task<IReadOnlyList<SelectItemViewModel>> ResolveAsync(string entityType, int[] entityIds,
        CancellationToken ct = default)
    {
        var result = new ConcurrentBag<SelectItemViewModel>();
        var idsToQuery = new ConcurrentBag<int>();

        foreach (var id in entityIds)
        {
            var cacheKey = $"EntityPermissions_{entityType}_{id}";
            var cachedItem = await cache.GetAsync<SelectItemViewModel>(cacheKey, ct);
            
            if (cachedItem != null)
            {
                result.Add(cachedItem);
            }
            else
            {
                idsToQuery.Add(id);
            }
        }

        if (idsToQuery.Any())
        {
            var queryResult = await QueryEntities(entityType, idsToQuery.ToArray(), ct);
            
            foreach (var item in queryResult)
            {
                result.Add(item);
                var cacheKey = $"EntityPermissions_{entityType}_{item.Id}";
                await cache.SetAsync(cacheKey, item, ct:ct);
            }
        }

        return result.ToList();
    }

    private async Task<IEnumerable<SelectItemViewModel>> QueryEntities(string entityType, int[] ids, CancellationToken ct)
    {
        switch (entityType)
        {
            case "Church":
                return await dbContext.Church
                    .AsNoTracking()
                    .Where(c => ids.Contains(c.Id))
                    .Select(c => new SelectItemViewModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description
                    })
                    .ToListAsync(ct);

            case "ChurchGroup":
                return await dbContext.ChurchGroup
                    .AsNoTracking()
                    .Where(c => ids.Contains(c.Id))
                    .Select(c => new SelectItemViewModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description
                    })
                    .ToListAsync(ct);

            case "Group":
                return await dbContext.Group
                    .AsNoTracking()
                    .Where(c => ids.Contains(c.Id))
                    .Select(c => new SelectItemViewModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description
                    })
                    .ToListAsync(ct);

            case "Person":
                return await dbContext.Person
                    .AsNoTracking()
                    .Where(c => ids.Contains(c.Id))
                    .Select(c => new SelectItemViewModel
                    {
                        Id = c.Id,
                        Name = c.FullName.ToString()
                    })
                    .ToListAsync(ct);

            default:
                return Array.Empty<SelectItemViewModel>();
        }
    }
}