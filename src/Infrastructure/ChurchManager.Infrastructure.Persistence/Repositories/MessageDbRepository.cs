using AutoMapper;
using ChurchManager.Domain.Common.Extensions;
using ChurchManager.Domain.Features;
using ChurchManager.Domain.Features.Communication;
using ChurchManager.Domain.Features.Communication.Repositories;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Persistence.Contexts;
using Convey.CQRS.Queries;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Repositories;

public class MessageDbRepository : GenericRepositoryBase<Message>, IMessageDbRepository
{
    private readonly IMapper _mapper;

    public MessageDbRepository(ChurchManagerDbContext dbContext, IMapper mapper) : base(dbContext)
    {
        _mapper = mapper;
    }

    public async Task<IList<MessageViewModel>> AllAsync(Guid userLoginId, IPagedQuery paging = null, CancellationToken ct = default)
    {
        return await Queryable()
            .AsNoTracking()
            .Where(n => n.UserId == userLoginId)
            .OrderByDescending(n => n.SentDateTime)
            .Skip(paging?.CalculateSkip() ?? 0)
            .Take(paging?.CalculateTake() ?? PagedQueryExtensions.DefaultPageSize)
            .Select(x => new MessageViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Body = x.Body,
                SentDateTime = x.SentDateTime.GetValueOrDefault(),
                IconCssClass = x.IconCssClass,
                Classification = x.Classification.ToString(),
                Link = x.Link,
                UseRouter = x.UseRouter,
                IsRead = x.IsRead,
                UserLoginId = x.UserId.ToString()
            })
            .ToListAsync(cancellationToken: ct);
    }

    public Task<int> UnreadCountAsync(Guid userLoginId, CancellationToken ct = default)
    {
        return Queryable()
            .AsNoTracking()
            .CountAsync(n =>n.UserId == userLoginId && !n.IsRead, cancellationToken: ct);
    }

    public async Task MarkAsReadAsync(int messageId, CancellationToken ct = default)
    {
        var message = await GetByIdAsync(messageId, ct);
        if (message is not null)
        {
            message.IsRead = true;
            await SaveChangesAsync(ct);
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
            .Where(n => n.UserId == userLoginId && !n.IsRead)
            .ExecuteUpdateAsync(s => s.SetProperty(b => b.IsRead, true), ct);
    }

    public async Task DeleteAllAsync(Guid userLoginId, CancellationToken ct = default)
    {
        var messages = await Queryable()
            .Where(n => n.UserId == userLoginId)
            .ToListAsync(ct);

        if (messages.Any())
        {
            await DeleteRangeAsync(messages, ct);
        }
    }

    public async Task<IList<Message>> PendingMessagesAsync(CancellationToken ct)
    {
        var messages = await Queryable()
            .Where(n => n.Status == MessageStatus.Pending)
            .ToListAsync(ct);

        return messages;
    }
}