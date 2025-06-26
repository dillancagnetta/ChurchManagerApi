namespace ChurchManager.Infrastructure.Shared.Xero;

public class XeroOptions
{
    public string ClientId { get; set; }

    public string ClientSecret { get; set; }

    public Uri CallbackUri { get; set; }

    public string Scopes { get; set; }
}