using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Features.Communications.Services;
using ChurchManager.Infrastructure.Shared.SMS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ChurchManager.Infrastructure.Tests;

public class BulkSmsSenderIntegration_Tests
{
    private readonly IConfiguration _configuration;
    private readonly IServiceCollection _services;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<BulkSmsSender> _logger;
    private readonly Mock<IHostEnvironment> _hostEnvironment = new Mock<IHostEnvironment>();
    private readonly ISmsSender _smsSender;


    public BulkSmsSenderIntegration_Tests()
    {
        // Build configuration manually with in-memory values
        var configValues = new Dictionary<string, string>
        {
            {$"{nameof(BulkSmsOptions)}:ApiUrl", "https://api.bulksms.com/v1/messages"},
            {$"{nameof(BulkSmsOptions)}:TokenId", "6167A0E81D4A4BF6A7F46BA2303394E0-01-A"},
            {$"{nameof(BulkSmsOptions)}:TokenSecret", "M3KD#wMlZkF2XAQJOvgNUeSMbKhqd"},
            {$"Application:SMSSendingEnabled", "true"}
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configValues)
            .Build();
        
        // Register services
        _services = new ServiceCollection();
        var installer = new SmsDependencyInstaller();
        installer.InstallServices(_services, _configuration, _hostEnvironment.Object);
        
        // Create a custom IHttpClientFactory
        //_httpClientFactory = new CustomHttpClientFactory(_configuration);

        // Create a logger (you can use NullLogger if you don't want to log in tests)
        //_logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<BulkSmsSender>();
        _services.AddLogging(builder => builder.AddConsole());
        
        var serviceProvider = _services.BuildServiceProvider();

        _smsSender = serviceProvider.GetRequiredService<ISmsSender>();

    }

    [Fact]
    public async Task SendSmsAsync_SuccessfulRequest_ReturnsSuccessResult()
    {
        // Arrange
        var message = new BulkSmsMessage
        {
            Body = "This is a test message from integration test.",
            From = "+27737378631",
            To = new List<SmsRecipient> {  new SmsRecipient()
            {
                PersonId = 1,
                PhoneNumber = "+27737378631"
            }},
            DeduplicationId = new Random().Next(1, 1000000)
        };

        // Act
        var result = await _smsSender.SendSmsAsync(message);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEmpty(result.Result);
    }
}