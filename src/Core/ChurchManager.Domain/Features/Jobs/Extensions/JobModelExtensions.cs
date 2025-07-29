using ChurchManager.Domain.Shared;
using CodeBoss.Jobs.Model;

namespace ChurchManager.Domain.Features.Jobs.Extensions;

public static class JobModelExtensions
{
    public static JobViewModel ToViewModel(this ServiceJob model)
    {
        return new JobViewModel
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            Assembly = model.Assembly,
            Class = model.Class,
            CronExpression = model.CronExpression,
            CronDescription = model.CronDescription,
            IsActive = model.IsActive,
            LastSuccessfulRunDateTime = model.LastSuccessfulRunDateTime,
            LastRunDateTime = model.LastRunDateTime,
            LastRunDurationSeconds = model.LastRunDurationSeconds,
            LastStatus = model.LastStatus,
            LastStatusMessage = model.LastStatusMessage,
            JobParameters = model.JobParameters,
            NotificationEmails = model.NotificationEmails,
            EnableHistory = model.EnableHistory,
            NotificationStatus = model.NotificationStatus switch
            {
                JobNotificationStatus.All => "All",
                JobNotificationStatus.Success => "Success", 
                JobNotificationStatus.Error => "Error",
                JobNotificationStatus.None => "None",
                _ => throw new ArgumentOutOfRangeException()
            },
            History = model.ServiceJobHistory.Select(h => h.ToViewModel()).ToList(),
            HistoryCount = model.ServiceJobHistory.Select(h => h.ToViewModel()).Count(),
        };
    }
    
    public static JobHistoryViewModel ToViewModel(this ServiceJobHistory model)
    {
        return new JobHistoryViewModel
        {
            Id = model.Id,
            StartDateTime = model.StartDateTime,
            StopDateTime = model.StopDateTime,
            Status = model.Status,
            StatusMessage = model.StatusMessage
        };
    }
}