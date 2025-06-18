using Wolverine;

namespace ChurchManager.Infrastructure.Abstractions.MassTransit
{
    public interface IUserHubService
    {
        ValueTask SendToUserAsync<TModel>(
            TModel model,
            string userId,
            string methodName,
            CancellationToken ct = default);
    }
}
