
using ChurchManager.Domain.Features.Settings;

namespace ChurchManager.Domain.Features.Communications;

public class BulkSmsSettings : ISettings
{
    public required string TokenId { get; set; }
    public required string TokenSecret { get; set; }
    public string ApiUrl { get; set; } = "https://api.bulksms.com/v1/messages";
}