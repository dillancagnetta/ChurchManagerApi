using CodeBoss.Jobs.Abstractions;
using CodeBoss.Jobs.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Infrastructure.Shared.Jobs;

/// <class>ChurchManager.Infrastructure.Shared.Jobs.TestJob</class>
/// <assembly>ChurchManager.Infrastructure.Shared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</assembly>
public class TestJob(IServiceJobRepository repository, ILogger<CodeBossJob> logger, IServiceScopeFactory scopeFactory) : CodeBossJob(repository, logger)
{
    public override async Task Execute(CancellationToken ct = default)
    {
        var jobParams = ServiceJob?.JobParameters;
        
        Console.Out.WriteLineAsync($"Hello from Job: [{nameof(TestJob)}]! at [{DateTime.Now}]" +
                                   $", has {jobParams.Count} parameters. [GroupId: {jobParams["groupId"]}]");
        
       

        UpdateLastStatusMessage($"Run at [{DateTime.Now}]");

        //await repository.SaveChangesAsync(ct);
    }
}