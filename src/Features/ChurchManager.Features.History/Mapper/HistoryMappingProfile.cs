using AutoMapper;
using ChurchManager.Application.ViewModels;
using ChurchManager.Infrastructure.Mapper;
using Convey.CQRS.Queries;

namespace ChurchManager.Features.History.Mapper;

public class HistoryMappingProfile : Profile, IAutoMapperProfile
{
    public int Order => 1;

    public HistoryMappingProfile()
    {
        CreateMap<Domain.Features.History.History, HistoryViewModel>()
            .ForMember(d => d.Date, opt => opt.MapFrom(src => src.CreatedDate))
            .ForMember(d => d.Entity, opt => opt.MapFrom(src => src.EntityType))
            .ForMember(d => d.RelatedEntity, opt => opt.MapFrom(src => src.RelatedEntityType))
            ;
        
        CreateMap<PagedResult<Domain.Features.History.History>, PagedResult<HistoryViewModel>>()
            .ForMember(d => d.Items,
                opt => opt.MapFrom(src => src.Items));
    }
}