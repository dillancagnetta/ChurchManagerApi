using System.Globalization;
using ChurchManager.Infrastructure.Abstractions.MultiTenancy;
using CodeBoss.Extensions;
using CodeBoss.Jobs;
using CodeBoss.Jobs.Abstractions;
using CodeBoss.Jobs.Jobs;
using CodeBoss.Jobs.Model;
using Codeboss.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;

namespace ChurchManager.Infrastructure.Shared.Jobs;

public class CmJobListener(
    IDateTimeProvider dateTime, 
    ITenantDbContextFactory tenantDbContextFactory,
    ILogger<CmJobListener> logger) : ICodeBossJobListener
{
    public string Name => nameof(CmJobListener);
    
    public async Task JobToBeExecuted(IJobExecutionContext context, CancellationToken ct = default)
    {
        // get job type id
        int serviceJobId = context.JobDetail.Description.AsInteger();
        string jobGroup = context.JobDetail.Key.Group;
        var tenantId = context.GetTenantIdFromQuartz();
        
        // Skip Job Pulse
        if (!tenantId.HasValue || jobGroup == "System")
        {
            logger.LogInformation("Skipping...No tenant ID or System Job {ServiceJobId}", serviceJobId);
            return;
        }
        
        // Use tenant-specific database context
        await using var dbContext = tenantDbContextFactory.CreateDbContext(tenantId.Value);
       
        // load ServiceJob from tenants database
        var job = await dbContext.Set<ServiceJob>()
            .FirstOrDefaultAsync(x => x.Id == serviceJobId, cancellationToken: ct);

        if (job != null)
        {
            var now = dateTime.Now;
            job.LastStatus = "Running";
            job.LastStatusMessage = "Started at " + now.ToString(CultureInfo.InvariantCulture);

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
            logger.LogInformation("Job '{JobName}' for tenant {TenantId} to be executed at {ExecutionTime}", 
                job.Name, tenantId, now);
            await dbContext.SaveChangesAsync(ct);
        }
    }

    public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken ct = default)
    {
        var tenantId = context.GetTenantIdFromQuartz();
        var tenantInfo = tenantId.HasValue ? $", Tenant: {tenantId}" : "";
        
        logger.LogDebug("Job ID: {JobId}, Job Key: {JobKey}{TenantInfo}, Job was vetoed.", 
            context.JobDetail?.Description.AsIntegerOrNull(), context.JobDetail?.Key, tenantInfo);
        
        return Task.CompletedTask;
    }

    public async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException,
        CancellationToken ct = default)
    {
        int serviceJobId = context.JobDetail.Description.AsInteger();
        var tenantId = context.GetTenantIdFromQuartz();
        string jobGroup = context.JobDetail.Key.Group;
        var codeBossJobInstance = context.JobInstance as CodeBossJob;
        
        // Skip Job Pulse
        if (!tenantId.HasValue || jobGroup == "System")
        {
            logger.LogInformation("Skipping...No tenant ID or System Job {ServiceJobId}", serviceJobId);
            return;
        }
        
        // Use tenant-specific database context
        await using var dbContext = tenantDbContextFactory.CreateDbContext(tenantId.Value);
        
        // load ServiceJob from tenants database
        var job = await dbContext.Set<ServiceJob>()
            .FirstOrDefaultAsync(x => x.Id == serviceJobId, cancellationToken: ct);

        if (job == null)
        {
            logger.LogDebug("Job ID: {JobId}, Job Key: {JobKey}, Tenant: {TenantId}, Job was not found.", 
                serviceJobId, context.JobDetail?.Key, tenantId);
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
            job.LastSuccessfulRunDateTime = job.LastRunDateTime;
            job.LastStatus = "Success";

            var result = codeBossJobInstance?.Result ?? context.Result as string;
            job.LastStatusMessage = result ?? string.Empty;

            // determine if message should be sent
            if ( job.NotificationStatus == JobNotificationStatus.Success )
            {
                sendMessage = true;
            }

            logger.LogDebug("Job ID: {JobId}, Job Key: {JobKey}, Tenant: {TenantId}, Job was executed.", 
                serviceJobId, context.JobDetail?.Key, tenantId);
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
                
            logger.LogDebug(exceptionToLog, "Job ID: {JobId}, Job Key: {JobKey}, Tenant: {TenantId}, Job was executed with an exception.", 
                serviceJobId, context.JobDetail?.Key, tenantId);
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
            SendNotificationMessage( jobException, job, tenantId.Value );
        }
    }

    private void SendNotificationMessage(JobExecutionException jobException, ServiceJob job, int tenantId)
    {
        // TODO: Implement tenant-aware notification logic
        logger.LogInformation("Notification needed for job {JobName} in tenant {TenantId}", job.Name, tenantId);
    }

    private Exception GetExceptionToLog( JobExecutionException jobException )
    {
        Exception exceptionToLog = jobException;

        // drill down to the interesting exception
        while ( exceptionToLog is SchedulerException && exceptionToLog.InnerException != null )
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