using ChurchManager.Domain.Features;
using ChurchManager.Domain.Features.Communication;
using ChurchManager.Domain.Features.Communication.Services;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Infrastructure.Abstractions.SignalR;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Infrastructure.Shared.Communications;

public class MessageSender : IMessageSender
{
    private readonly IConnectionTracker _tracker;
    private readonly IUserNotificationsHubService _pushNotify;
    private readonly IUserLoginDbRepository _userLogin;
    private readonly IWebPushService _webPush;
    private readonly ILogger<MessageSender> _logger;

    public MessageSender(
        IConnectionTracker tracker,
        IUserNotificationsHubService pushNotify,
        IUserLoginDbRepository userLogin,
        IWebPushService webPush,
        ILogger<MessageSender> logger)
    {
        _tracker = tracker;
        _pushNotify = pushNotify;
        _userLogin = userLogin;
        _webPush = webPush;
        _logger = logger;
    }
    
    public async Task SendAsync(Message message, CancellationToken ct = default)
    {
        try
        {
            // Check if user is connected via SignalR
            bool isUserConnected = await _tracker.IsUserConnectedAsync(message.UserId.ToString());
            if (isUserConnected)
            {
                await _pushNotify.SendMessageToUserAsync(message, ct);
            }
            else if (message.SendWebPush)
            {
                var userLogin = await _userLogin.GetByIdAsync(message.UserId, ct);
                var notification = new PushNotification(message);
                await _webPush.SendNotificationAsync(userLogin.PersonId, notification, ct);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw;
        }
    }
    
}