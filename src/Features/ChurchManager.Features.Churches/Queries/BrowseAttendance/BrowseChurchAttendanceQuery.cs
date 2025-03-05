using ChurchManager.Application.Abstractions.Services;
using ChurchManager.Domain.Parameters;
using ChurchManager.Domain.Shared;
using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.Churches.Queries.BrowseAttendance
{
    public record BrowseChurchAttendanceQuery : QueryParameter, IRequest<PagedResponse<ChurchAttendanceViewModel>>
    {
        public int[] AttendanceTypeIds { get; set; }
        public int ChurchId { get; set; }
        public int? ChurchGroupId { get; set; }
        public bool WithFeedBack { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }

    public class BrowseChurchAttendanceQueryHandler(IChurchService service) : IRequestHandler<BrowseChurchAttendanceQuery, PagedResponse<ChurchAttendanceViewModel>>
    {
        
        public async Task<PagedResponse<ChurchAttendanceViewModel>> Handle(BrowseChurchAttendanceQuery query, CancellationToken ct)
        {
            var results = await service.BrowseChurchAttendance(
                query,
                query.AttendanceTypeIds,
                query.ChurchId,
                query.ChurchGroupId,
                query.WithFeedBack,
                query.From, query.To, ct);

            return new PagedResponse<ChurchAttendanceViewModel>(results);
        }
    }
}