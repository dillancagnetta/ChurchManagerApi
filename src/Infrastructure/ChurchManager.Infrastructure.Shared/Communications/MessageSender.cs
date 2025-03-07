using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Features.Communications.Repositories;
using ChurchManager.Domain.Features.Communications.Services;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Infrastructure.Abstractions.SignalR;
using Codeboss.Types;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Infrastructure.Shared.Communications;

public class MessageSender : IMessageSender
{
    private readonly IConnectionTracker _tracker;
    private readonly IUserNotificationsHubService _pushNotify;
    private readonly IMessageDbRepository _messageDb;
    private readonly IDateTimeProvider _dateTime;
    private readonly IUserLoginDbRepository _userLogin;
    private readonly IWebPushService _webPush;
    private readonly ILogger<MessageSender> _logger;

    public MessageSender(
        IConnectionTracker tracker,
        IUserNotificationsHubService pushNotify,
        IMessageDbRepository messageDb,
        IDateTimeProvider dateTime,
        IUserLoginDbRepository userLogin,
        IWebPushService webPush,
        ILogger<MessageSender> logger)
    {
        _tracker = tracker;
        _pushNotify = pushNotify;
        _messageDb = messageDb;
        _dateTime = dateTime;
        _userLogin = userLogin;
        _webPush = webPush;
        _logger = logger;
    }
    
    public async Task SendAsync(Message message, CancellationToken ct = default)
    {
        bool isSent = false;
        try
        {
            // Check if user is connected via SignalR
            bool isUserConnected = await _tracker.IsUserConnectedAsync(message.UserId.ToString());
            if (isUserConnected)
            {
                _logger.LogInformation($"📍 User: [{message.UserId}] connected, sending push notification.");
                await _pushNotify.SendMessageToUserAsync(message, ct);
                isSent = true;
            }
            else if (message.SendWebPush)
            {
                _logger.LogInformation($"📍 User: [{message.UserId}] not connected, sending [web] push notification.");
                var userLogin = await _userLogin.GetByIdAsync(message.UserId, ct);
                var notification = new PushNotification(message);
                await _webPush.SendNotificationAsync(userLogin.PersonId, notification, ct);
                isSent = true;
            }

            if (isSent)
            {
                message.MarkAsSent(_dateTime);
                await _messageDb.SaveChangesAsync(ct);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            message.LastError = ex.Message;
            await _messageDb.SaveChangesAsync(ct);
            throw;
        }
    }
    
}