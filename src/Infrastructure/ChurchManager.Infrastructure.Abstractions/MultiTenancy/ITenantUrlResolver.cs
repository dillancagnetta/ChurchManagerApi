namespace ChurchManager.Infrastructure.Abstractions.MultiTenancy;

public interface ITenantUrlResolver
{
    string CurrentTenantHostUrl();
    string TenantApiUrl(string tenantName);
    string WebhookUrl(string tenantName, string webhookPath);
    string CurrentTenantWebhookUrl(string webhookPath);
    string CurrentTenantSubDomainUrl(string path);
}