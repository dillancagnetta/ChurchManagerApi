using ChurchManager.Infrastructure.Abstractions.Finances;
using ChurchManager.Infrastructure.Abstractions.Persistence;
using Codeboss.Results;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xero.NetStandard.OAuth2.Api;
using Xero.NetStandard.OAuth2.Client;
using Xero.NetStandard.OAuth2.Config;
using Xero.NetStandard.OAuth2.Token;

namespace ChurchManager.Infrastructure.Shared.Xero;

public class XeroFinanceService : IExternalFinanceIntegrator
{
    private const string CacheKey = "XeroAccessToken";
    private readonly IQueryCache _cache;
    private readonly ILogger<XeroFinanceService> _logger;
    private readonly XeroConfiguration _configuration;
    
    public XeroFinanceService(
        IQueryCache cache,
        IOptions<XeroOptions> options,
        ILogger<XeroFinanceService> logger)
    {
        _cache = cache;
        _logger = logger;
        var _options = options.Value;
        _configuration = new XeroConfiguration
        {
            ClientId = _options.ClientId,
            ClientSecret = _options.ClientSecret,
            CallbackUri = _options.CallbackUri,
            Scope = _options.Scopes
        };
    }
    
    public string AuthUrl()
    {
        var client = new XeroClient(_configuration);
        return client.BuildLoginUri();
    }

    public async Task<OperationResult<string>> RequestAccessTokenAsync(string code, CancellationToken ct = default)
    {
        try
        {
            var client = new XeroClient(_configuration);
            //var token = await client.RequestAccessTokenAsync(code);

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            };
            var token = await _cache.GetOrSetAsync<IXeroToken>(CacheKey, 
                 () =>  client.RequestAccessTokenAsync(code), cacheOptions, ct);
            
            /*
            var client = new HttpClient();
            var values = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", "http://localhost:5001/api/v1/finances/xero-callback" },
                { "client_id", _configuration.ClientId },
                { "client_secret", _configuration.ClientSecret }
            };

            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("https://identity.xero.com/connect/token", content);
            var responseString = await response.Content.ReadAsStringAsync();
            */
            
        
            // Store information about the connected organization
            if (token.Tenants?.Any() == true)
            {
                var tenant = token.Tenants[0];
                _logger.LogInformation($"Connected to Xero organization: {tenant.TenantName}");
            }
        
            return OperationResult<string>.Success(token.AccessToken);
        }
        catch (Exception e)
        {
            return OperationResult<string>.Fail(e.Message);
        }
    }

    public async Task<bool> HasValidTokenAsync(CancellationToken ct = default)
    {
        var client = await GetConnectedClientAsync();
        return client != null;
    }

    public async Task<IEnumerable<T>> GetContactsAsync<T>(CancellationToken ct = default)
    {
        var client = await GetConnectedClientAsync(ct);
        if (client == null)
            throw new InvalidOperationException("Not connected to Xero");
    
        /*new AccountingApi().GetContactsAsync
        var contacts = await client.GetContactsAsync(client.TenantId);
        return contacts._Contacts;*/
        return [];
    }

    public Task<T> CreateOrUpdateContactAsync<T>(T contact, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
    
    private async Task<XeroClient?> GetConnectedClientAsync(CancellationToken ct = default)
    {
        var token = await _cache.GetAsync<IXeroToken?>(CacheKey, ct);
        //var token = await _tokenStore.GetStoredTokenAsync();
        if (token == null)
        {
            _logger.LogWarning("No Xero token found in storage");
            return null;
        }
    
        // Check if token is expired or about to expire
        if (token.ExpiresAtUtc < DateTime.UtcNow.AddMinutes(5))
        {
            _logger.LogInformation("Access token expired or expiring soon, refreshing...");
        
            try
            {
                var client = new XeroClient(_configuration);
                token = await client.RefreshAccessTokenAsync(token);
                
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = token.ExpiresAtUtc
                };
                await _cache.SetAsync<IXeroToken?>(CacheKey, token, cacheOptions, ct);
                
                _logger.LogInformation("Successfully refreshed access token");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to refresh access token");
                return null;
            }
        }
    
        // Create and configure API client with token
        var connectedClient = new XeroClient(_configuration);
        await connectedClient.GetCurrentValidTokenAsync(token);
    
        return connectedClient;
    }
}