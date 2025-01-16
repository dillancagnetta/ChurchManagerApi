namespace ChurchManager.Infrastructure.Abstractions.SignalR;

public interface IConnectionTracker
{
    Task AddConnectionAsync(string userId, string connectionId);
    Task RemoveConnectionAsync(string userId, string connectionId);
    Task<bool> IsUserConnectedAsync(string userId);
}