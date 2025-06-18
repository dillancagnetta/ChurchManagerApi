using System.Text;
using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Features.Communications.Repositories;
using ChurchManager.Domain.Features.Communications.Services;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Shared;
using CodeBoss.Extensions;
using CodeBoss.Jobs.Abstractions;
using CodeBoss.Jobs.Jobs;
using Codeboss.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;

namespace ChurchManager.Domain.Features.Groups.Jobs;

public class NotifyWorstGroupMemberAttendanceJob(
    IServiceJobRepository repository, 
    ILogger<CodeBossJob> logger,
    IGroupAttendanceDbRepository groupAttendance,
    IMessageDbRepository messageDb,
    IGroupMemberDbRepository groupMembers,
    IPersonDbRepository personDb,
    IPushNotificationsService pusher
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

            var leaders = await groupMembers.GetLeaders(groupId.Value).ToListAsync(ct);
            
            Result = GetJobResultMessages(worstAttendees);
            
            //await SendPushNotificationsAsync(leaders, worstAttendees, ct);
            await AddMessagesToDbForLeadersAsync(leaders, worstAttendees, ct);
            
            await UpdateLastStatusMessage(Result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            throw new JobExecutionException(msg: ex.Message, refireImmediately: false, cause: ex);
        }
    }

    /// <summary>
    /// Adding messages to the database will trigger sending.
    /// </summary>
    private async Task AddMessagesToDbForLeadersAsync(List<GroupMember> leaders, List<GroupMemberAttendanceRate> worstAttendees, CancellationToken ct)
    {
        foreach (var leader in leaders)
        {
            var operationResult = await personDb.UserLoginIdForPersonAsync(leader.PersonId, ct);
            if (operationResult.IsSuccess && operationResult.Result.HasValue)
            {
                var userLoginId = operationResult.Result.Value;
                var message = Message.CreateWarningMessage("Worst Group Attendees", Result, userLoginId);
                message.SendWebPush = true;
                
                await messageDb.AddAsync(message, ct);
            }
        }
    }

    private async Task SendPushNotificationsAsync(List<GroupMember> leaders, List<GroupMemberAttendanceRate> worstAttendees, CancellationToken ct)
    {
        foreach (var leader in leaders)
        {
            var operationResult = await personDb.UserLoginIdForPersonAsync(leader.PersonId, ct);
            if (operationResult.IsSuccess && operationResult.Result.HasValue)
            {
                var userLoginId = operationResult.Result.Value;
                // Send push notification using a notification service
                var notification = Notification.UserNotification(
                    type:"success", 
                    title:"Worst Group Attendees", 
                    payload:Result, 
                    methodName:"DirectMessage", 
                    userId:userLoginId.ToString());
                
                await pusher.PushAsync(notification, ct);
            }
        }
    }

    private string GetJobResultMessages(IList<GroupMemberAttendanceRate> worstAttendees)
    {
        var results = new StringBuilder();
        if (worstAttendees.Any())
        {
            worstAttendees.ForEach( e => results.AppendLine( $"{e.MemberName} -[AttendanceRate]:{e.AttendanceRatePercent}%" ) );
        }
        return results.ToString();
    }
}