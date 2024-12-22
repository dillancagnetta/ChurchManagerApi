using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ChurchManager.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace ChurchManager.Infrastructure.Persistence.Repositories
{
    public class GroupMemberAttendanceDbRepository : GenericRepositoryBase<GroupMemberAttendance>, IGroupMemberAttendanceDbRepository
    {
        public GroupMemberAttendanceDbRepository(ChurchManagerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<(int newConvertsCount, int firstTimersCount, int holySpiritCount)> PeopleStatisticsAsync(IEnumerable<int> groupIds, DateTime startDate = default)
        {
            if (startDate == default)
            {
                startDate = DateTime.UtcNow.AddMonths(-6);
            }

            var query = Queryable()
                .Where(x =>
                    groupIds.Contains(x.GroupId) &&
                    x.AttendanceDate >= startDate);

            var newConvertsCount = await query.CountAsync(x => x.IsNewConvert.HasValue && x.IsNewConvert.Value);
            var firstTimersCount = await query.CountAsync(x => x.IsFirstTime.HasValue && x.IsFirstTime.Value);
            var holySpiritCount = await query.CountAsync(x => x.ReceivedHolySpirit.HasValue && x.ReceivedHolySpirit.Value);

            return (newConvertsCount, firstTimersCount, holySpiritCount);
        }

        public async Task<List<MemberAttendanceReport>> MemberAttendanceReportAsync(int groupId, CancellationToken ct = default)
        {
            var connection = DbContext.Database.GetDbConnection();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM get_group_attendance_report(@p0)";
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@p0";
            parameter.Value = groupId;
            command.Parameters.Add(parameter);

            try
            {
                await connection.OpenAsync(ct);
                using var result = await command.ExecuteReaderAsync(ct);
                var reports = new List<MemberAttendanceReport>();
        
                while (await result.ReadAsync(ct))
                {
                    reports.Add(new MemberAttendanceReport
                    {
                        GroupId = result.GetInt32(0),
                        GroupMemberId = result.GetInt32(1),
                        PersonId = result.GetInt32(2),
                        PersonFullName = result.IsDBNull(3) ? null : result.GetString(3),
                        PhotoUrl = result.IsDBNull(4) ? null : result.GetString(4),
                        Meeting1 = result.GetBoolean(5),
                        Meeting2 = result.GetBoolean(6),
                        Meeting3 = result.GetBoolean(7),
                        Meeting4 = result.GetBoolean(8),
                        Meeting5 = result.GetBoolean(9),
                        AttendanceRatePercent = result.GetDecimal(10)
                    });
                }
        
                return reports;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }
    }
}
