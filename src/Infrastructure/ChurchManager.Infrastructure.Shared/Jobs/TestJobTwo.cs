using CodeBoss.Jobs.Abstractions;
using CodeBoss.Jobs.Jobs;
using Microsoft.Extensions.Logging;

namespace ChurchManager.Infrastructure.Shared.Jobs;

/// <class>ChurchManager.Infrastructure.Shared.Jobs.TestJobTwo</class>
/// <assembly>ChurchManager.Infrastructure.Shared, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</assembly>
public class TestJobTwo(IServiceJobRepository repository, ILogger<CodeBossJob> logger) : CodeBossJob(repository, logger)
{
    public override async Task Execute(CancellationToken ct = default)
    {
        var jobParams = ServiceJob?.JobParameters;
        
        Console.Out.WriteLineAsync($"Hello from TestJobTwo, Cron description: [{ServiceJob?.CronDescription}]" +
                                   $", has {jobParams?.Count ?? 0} parameters.");
        
        
        UpdateStatusMessagesAsync($"Run at [{DateTime.Now}]", "Success");

        //await repository.SaveChangesAsync(ct);
    }
}