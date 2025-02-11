using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Features.Communications.Repositories;
using ChurchManager.Domain.Features.Communications.Services;

namespace ChurchManager.Infrastructure.Shared.Communications;

public class WebPushService : IWebPushService
{
    private readonly IPushDeviceDbRepository _dbRepository;
    private readonly IWebPushSenderClient _client;

    public WebPushService(
        IPushDeviceDbRepository dbRepository,
        IWebPushSenderClient client)
    {
        _dbRepository = dbRepository;
        _client = client;
    }
    
    public async Task SendNotificationAsync(int personId, PushNotification notification, CancellationToken ct = default)
    {
        var devices = await _dbRepository.PushDevicesForPersonAsync(personId, ct);

        foreach (var device in devices)
        {
            await _client.SendNotificationAsync(device, notification, ct);
        }
    }

    public Task SendNotificationAsync(string userId, PushNotification notification, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}