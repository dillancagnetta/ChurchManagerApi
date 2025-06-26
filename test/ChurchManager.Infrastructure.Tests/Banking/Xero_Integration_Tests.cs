/*using ChurchManager.Infrastructure.Abstractions.Persistence;
using ChurchManager.Infrastructure.Shared.Xero;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace ChurchManager.Infrastructure.Tests.Xero;

public class Xero_Integration_Tests
{
    private const string ClientId = "4229830C5DAC43CBA69B08FA05A31D9E";
    private const string ClientSecret = "9Ex9oKscOoU0pYr3efzpQ7SH1Yxn7Mvyi_gJqbTA6hDWFLhW";
    private const string CallbackUri = "http://localhost:5001/api/v1/xero/callback";
    private const string Scopes = "accounting.transactions accounting.contacts accounting.settings offline_access";

    private IOptions<XeroOptions> _options = null;

    public Xero_Integration_Tests()
    {
        _options = Options.Create(new XeroOptions
        {
            ClientId = ClientId,
            ClientSecret = ClientSecret,
            CallbackUri = new Uri(CallbackUri),
            Scopes = Scopes
        });
    }

    [Fact]
    public void Test_Xero_Get_AuthUrl()
    {

        var cache = new Mock<IQueryCache>().Object;
        var sut = new XeroFinanceService(cache, _options, NullLogger<XeroFinanceService>.Instance);
        var result = sut.AuthUrl();
            
        Assert.NotEmpty(result);
    }
    
    [Fact]
    public async Task Test_Xero_RequestAccessToken()
    {
        var cache = new Mock<IQueryCache>().Object;
        var sut = new XeroFinanceService(cache, _options, NullLogger<XeroFinanceService>.Instance);
        var result = await sut.RequestAccessTokenAsync("bH_uGlJpvYv2WrguVNtu2HgAIHN-IfIQEsnWgBZIDRI", CancellationToken.None);
            
        Assert.NotEmpty(result.Result);
    }
}*/