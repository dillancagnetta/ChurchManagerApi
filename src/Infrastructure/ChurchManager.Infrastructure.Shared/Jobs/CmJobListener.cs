using ChurchManager.Infrastructure.Persistence.Contexts;
using CodeBoss.Extensions;
using CodeBoss.Jobs.Abstractions;
using CodeBoss.Jobs.Jobs;
using CodeBoss.Jobs.Model;
using Codeboss.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;

namespace ChurchManager.Infrastructure.Shared.Jobs;

public class CmJobListener(IServiceProvider serviceProvider, IDateTimeProvider dateTime, ILogger<CmJobListener> logger) : ICodeBossJobListener
{
    public string Name => nameof(CmJobListener);
    
    public async Task JobToBeExecuted(IJobExecutionContext context, CancellationToken ct = default)
    {
        // get job type id
        int serviceJobId = context.JobDetail.Description.AsInteger();
    
        using var scope = serviceProvider.CreateScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<ChurchManagerDbContext>();
        // load ServiceJob from database
        var job = await dbContext.Set<ServiceJob>()
            .FirstOrDefaultAsync(x => x.Id == serviceJobId, cancellationToken: ct);

        if (job != null)
        {
            var now = dateTime.Now;
            job.LastStatus = "Running";
            job.LastStatusMessage = "Started at " + now.ToString();

            if (job.EnableHistory)
            {
                var history = new ServiceJobHistory
                {
                    ServiceJobId = job.Id,
                    StartDateTime = now,
                    StopDateTime = null,
                    Status = job.LastStatus,
                    StatusMessage = job.LastStatusMessage,
                };
                job.ServiceJobHistory.Add(history);
            }
            logger.LogInformation($"Job '{job.Name}' to be executed at {now}");
            await dbContext.SaveChangesAsync(ct);
        }
    }

    public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException,
        CancellationToken ct = default)
    {
        int serviceJobId = context.JobDetail.Description.AsInteger();
        var codeBossJobInstance = context.JobInstance as CodeBossJob;

        using var scope = serviceProvider.CreateScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<ChurchManagerDbContext>();
        // load ServiceJob from database
        var job = await dbContext.Set<ServiceJob>()
            .FirstOrDefaultAsync(x => x.Id == serviceJobId, cancellationToken: ct);

        if (job == null)
        {
            // log
            return;
        }
            
        // if notification status is all set flag to send message
        bool sendMessage = job.NotificationStatus == JobNotificationStatus.All;
            
        // set last run date
        job.LastRunDateTime = dateTime.Now;
            
        // set run time
        job.LastRunDurationSeconds = Convert.ToInt32( context.JobRunTime.TotalSeconds );

        // determine if an error occurred
        if ( jobException == null )
        {
            //job.LastSuccessfulRunDateTime = job.LastRunDateTime;
            job.LastStatus = "Success";

            var result = codeBossJobInstance?.Result ?? context.Result as string;
            job.LastStatusMessage = result ?? string.Empty;

            // determine if message should be sent
            if ( job.NotificationStatus == JobNotificationStatus.Success )
            {
                sendMessage = true;
            }

            logger.LogInformation($"Job '{job.Name}' executed successfully at {dateTime.Now}");
        }
        else
        {
            var exceptionToLog = GetExceptionToLog( jobException );
            // log error

            if (exceptionToLog != null )
            {
                // put the exception into the status
                job.LastStatus = "Exception";
                if ( exceptionToLog is AggregateException { InnerExceptions.Count: > 1 } aggregateException )
                {
                    var firstException = aggregateException.InnerExceptions.First();
                    job.LastStatusMessage = "One or more exceptions occurred. First Exception: " + firstException.Message;
                }
                else
                {
                    job.LastStatusMessage = exceptionToLog.Message;
                }
            }
                
            if ( job.NotificationStatus == JobNotificationStatus.Error )
            {
                sendMessage = true;
            }
                
            logger.LogInformation($"Job '{job.Name}' exception at {dateTime.Now} with {exceptionToLog.Message}");
        }
            
        await dbContext.SaveChangesAsync(ct);
            
        if ( job.EnableHistory )
        {
            var history = new ServiceJobHistory
            {
                ServiceJobId = job.Id,
                StartDateTime =  GetStartedDateTimeForLastRun( job ),
                StopDateTime = job.LastRunDateTime,
                Status = job.LastStatus,
                StatusMessage = job.LastStatusMessage,
            };

            job.ServiceJobHistory.Add(history);
                
            await dbContext.SaveChangesAsync(ct);
        }
            
        // send notification
        if ( sendMessage )
        {
            SendNotificationMessage( jobException, job );
        }
    }

    private void SendNotificationMessage(JobExecutionException jobException, ServiceJob job)
    {
    }

    private Exception GetExceptionToLog( JobExecutionException jobException )
    {
        Exception exceptionToLog = jobException;

        // drill down to the interesting exception
        while ( exceptionToLog is Quartz.SchedulerException && exceptionToLog.InnerException != null )
        {
            exceptionToLog = exceptionToLog.InnerException;
        }

        AggregateException aggregateException = exceptionToLog as AggregateException;
        if ( aggregateException != null && aggregateException.InnerExceptions != null && aggregateException.InnerExceptions.Count == 1 )
        {
            // if it's an aggregate, but there is only one, convert it to a single exception
            exceptionToLog = aggregateException.InnerExceptions[0];
            aggregateException = null;
        }

        return exceptionToLog;
    }
    
    private DateTime? GetStartedDateTimeForLastRun( ServiceJob job )
    {
        return job.LastRunDateTime?.AddSeconds( -( job.LastRunDurationSeconds ?? 0 ) );
    }
}