﻿using System.Linq.Dynamic.Core;
using ChurchManager.Domain.Common.Extensions;
using ChurchManager.Domain.Features;
using ChurchManager.Domain.Features.Communication.Repositories;
using ChurchManager.Infrastructure.Persistence.Contexts;
using Convey.CQRS.Queries;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Repositories;

public class MessageDbRepository : GenericRepositoryBase<Message>, IMessageDbRepository
{
    public MessageDbRepository(ChurchManagerDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Message>> AllAsync(Guid userLoginId, IPagedQuery paging = null, CancellationToken ct = default)
    {
        return await Queryable()
            .AsNoTracking()
            .Where(n => n.UserLoginId == userLoginId)
            .OrderByDescending(n => n.SentDateTime)
            .Skip(paging?.CalculateSkip() ?? 0)
            .Take(paging?.CalculateTake() ?? PagedQueryExtensions.DefaultPageSize)
            .ToListAsync(cancellationToken: ct);
    }

    public Task<int> UnreadCountAsync(Guid userLoginId, CancellationToken ct = default)
    {
        return Queryable()
            .AsNoTracking()
            .CountAsync(n =>n.UserLoginId == userLoginId && !n.IsRead, cancellationToken: ct);
    }

    public async Task MarkAsReadAsync(int messageId, CancellationToken ct = default)
    {
        var message = await GetByIdAsync(messageId, ct);
        if (message is not null)
        {
            message.IsRead = true;
        }
    }

    public async Task DeleteAsync(int messageId, CancellationToken ct = default)
    {
        var message = await GetByIdAsync(messageId, ct);
        if (message is not null)
        {
            await DeleteAsync(message, ct);
        }
    }

    public Task MarkAllAsReadAsync(Guid userLoginId, CancellationToken ct = default)
    {
        return Queryable()
            .Where(n => n.UserLoginId == userLoginId && !n.IsRead)
            .ExecuteUpdateAsync(s => s.SetProperty(b => b.IsRead, true), ct);
    }

    public async Task DeleteAllAsync(Guid userLoginId, CancellationToken ct = default)
    {
        var messages = await Queryable()
            .Where(n => n.UserLoginId == userLoginId)
            .ToListAsync(ct);

        if (messages.Any())
        {
            await DeleteRangeAsync(messages, ct);
        }
    }
}