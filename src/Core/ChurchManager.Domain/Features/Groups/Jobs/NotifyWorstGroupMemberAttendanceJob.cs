using System.Text;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Shared;
using CodeBoss.Extensions;
using CodeBoss.Jobs.Abstractions;
using CodeBoss.Jobs.Jobs;
using Microsoft.Extensions.Logging;
using Quartz;

namespace ChurchManager.Domain.Features.Groups.Jobs;

public class NotifyWorstGroupMemberAttendanceJob(
    IServiceJobRepository repository, 
    ILogger<CodeBossJob> logger,
    IGroupAttendanceDbRepository groupAttendance
    )
    : CodeBossJob(repository, logger)
{
    public override async Task Execute(CancellationToken ct = default)
    {
        try
        {
            var groupId = ServiceJob?.JobParameters["groupId"].AsInteger(); // Replace with actual group ID
            var worstAttendees = await groupAttendance.TopWorstAttendeesAsync(groupId.Value, top:3, ct);
            logger.LogInformation($"[{nameof(NotifyWorstGroupMemberAttendanceJob)}] Worst attendees: {worstAttendees.Count}");
            
            Result = GetJobResultMessages(worstAttendees);
            await UpdateLastStatusMessage(Result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            throw new JobExecutionException(msg: ex.Message, refireImmediately: false, cause: ex);
        }
    }

    private string GetJobResultMessages(IList<GroupMemberAttendanceRate> worstAttendees)
    {
        var results = new StringBuilder();
        if (worstAttendees.Any())
        {
            worstAttendees.ForEach( e => results.AppendLine( $"{e.MemberName} - {e.AttendanceRatePercent}" ) );
        }
        return results.ToString();
    }
}