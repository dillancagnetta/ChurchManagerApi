using System.Text;
using ChurchManager.Domain.Features.Communication;
using ChurchManager.Domain.Features.Communication.Repositories;
using ChurchManager.Domain.Features.Communication.Services;
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
    IUserLoginDbRepository userLogins,
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

    private async Task AddMessagesToDbForLeadersAsync(List<GroupMember> leaders, List<GroupMemberAttendanceRate> worstAttendees, CancellationToken ct)
    {
        foreach (var leader in leaders)
        {
            var userLogin = await userLogins.UserLoginIdForAsync(leader.PersonId, ct);
            if (userLogin.HasValue)
            {
                var message = Message.CreateWarningMessage("Worst Group Attendees", Result, userLogin.Value);
                message.SendWebPush = true;
                
                await messageDb.AddAsync(message, ct);
            }
        }
    }

    private async Task SendPushNotificationsAsync(List<GroupMember> leaders, List<GroupMemberAttendanceRate> worstAttendees, CancellationToken ct)
    {
        foreach (var leader in leaders)
        {
            var userLogin = await userLogins.UserLoginIdForAsync(leader.PersonId, ct);
            if (userLogin.HasValue)
            {
                // Send push notification using a notification service
                var notification = Codeboss.Types.Notification.UserNotification(
                    type:"success", 
                    title:"Worst Group Attendees", 
                    payload:Result, 
                    methodName:"DirectMessage", 
                    userId:userLogin.Value.ToString());
                
                await pusher.PushAsync(notification, ct);
            }
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