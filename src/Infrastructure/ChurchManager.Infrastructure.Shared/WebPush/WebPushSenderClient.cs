using ChurchManager.Domain.Features.Communication;
using ChurchManager.Domain.Features.Communication.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebPush;
using PushSubscription = WebPush.PushSubscription;

namespace ChurchManager.Infrastructure.Shared.WebPush
{
    public class WebPushSenderClient : IWebPushSenderClient
    {
        private readonly WebPushClient _client;

        private readonly JsonSerializerSettings _jsonSerializerSettings = new()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public WebPushSenderClient(WebPushClient client) => _client = client;

        public async Task SendNotificationAsync(PushDevice device, PushNotification notification, CancellationToken ct = default)
        {
            var subscription = new PushSubscription(device.Endpoint, device.P256DH, device.Auth);

            var payload = JsonConvert.SerializeObject(new { notification }, _jsonSerializerSettings);

            await _client.SendNotificationAsync(subscription, payload, cancellationToken: ct);
        }
    }
}
