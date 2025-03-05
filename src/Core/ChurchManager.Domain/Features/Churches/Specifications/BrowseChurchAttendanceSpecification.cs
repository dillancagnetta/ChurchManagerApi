using Ardalis.Specification;
using ChurchManager.Domain.Common.Extensions;
using ChurchManager.Domain.Shared;
using ChurchManager.Domain.Specifications;
using CodeBoss.Extensions;
using Convey.CQRS.Queries;

namespace ChurchManager.Domain.Features.Churches.Specifications
{
    public class BrowseChurchAttendanceSpecification : PermissionSpecification<ChurchAttendance, ChurchAttendanceViewModel>
    {
        // When the churchId is 0 - it means we want to include all churches
        private const int AllChurchesId = 0;

        public BrowseChurchAttendanceSpecification(
            IPagedQuery paging,
            int[] attendanceTypeIds,
            int churchId,
            int? churchGroupId,
            bool withFeedback,
            DateTime? from, DateTime? to)
        {
            Query.AsNoTracking();
            Query.Include("Church.ChurchGroup");
            Query.Include("ChurchAttendanceType");

            // Type Filter
            if (!attendanceTypeIds.IsNullOrEmpty())
            {
                Query.Where(g => attendanceTypeIds.Contains(g.ChurchAttendanceTypeId));
            }

            
            // Group Filter
            if (churchGroupId.HasValue)
            {
                Query.Where(g => g.Church.ChurchGroupId == churchGroupId);
            }
            
            // Church Filter
            if(churchId != AllChurchesId)
            {
                Query.Where(g => g.ChurchId == churchId);
            }
            

            // Date Filters
            if(from.HasValue)
            {
                Query.Where(g => g.AttendanceDate >= from.Value);
            }
            if(to.HasValue)
            {
                Query.Where(g => g.AttendanceDate <= to.Value);
            }

            Query.OrderByDescending(x => x.AttendanceDate);

            Query
                .Skip(paging.CalculateSkip())
                .Take(paging.CalculateTake());

            Query.Select(x => new ChurchAttendanceViewModel
            {
                Id = x.Id,
                ChurchName = x.Church.Name,
                ChurchGroupName = x.Church.ChurchGroup.Name,
                AttendanceTypeName = x.ChurchAttendanceType.Name,
                AttendanceDate = x.AttendanceDate,
                DidNotOccur = x.DidNotOccur,
                AttendanceCount = x.AttendanceCount,
                FirstTimerCount = x.FirstTimerCount,
                NewConvertCount = x.NewConvertCount,
                ReceivedHolySpiritCount = x.ReceivedHolySpiritCount,
                Notes = x.Notes,
                PhotoUrls = x.PhotoUrls,
                //Offering = x.Amount.HasValue ? new MoneyViewModel{Currency = x.Currency, Amount = x.Amount.Value} : null
            });
        }  
    }
}
