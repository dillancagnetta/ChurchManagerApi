using AutoMapper;
using ChurchManager.Domain.Features.Events;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Mapper;

namespace ChurchManager.Features.Events.Infrastructure.Mapper;

public class EventsMappingProfile: Profile, IAutoMapperProfile
{
    public EventsMappingProfile()
    {
        CreateMap<EventViewModel, Event>();

        CreateMap<Event, EventViewModel>()
            .ForMember(d => d.ChurchName, opt =>
                opt.MapFrom(src => src.Church != null ? src.Church.Name : null))
            .ForMember(d => d.ChurchGroupName, opt =>
                opt.MapFrom(src => src.Church.ChurchGroup != null ? src.Church.ChurchGroup.Name : null))
            .ForMember(d => d.EventTypeName, opt =>
                opt.MapFrom(src => src.EventType != null ? src.EventType.Name : null))
            .ForMember(d => d.ScheduleFriendlyText, opt =>
                opt.MapFrom(src => src.Schedule != null ? src.Schedule.ToFriendlyScheduleText(false) : null))
            .ForMember(d => d.StartDate, opt =>
                opt.MapFrom(src => src.Schedule != null ? src.Schedule.StartDate.GetValueOrDefault() : (DateTime?) null))
            .ForMember(d => d.EndDate, opt =>
                opt.MapFrom(src => src.Schedule != null ? src.Schedule.EndDate.GetValueOrDefault() : (DateTime?) null))
            .ForMember(d => d.NumberOfSessions, opt =>
                opt.MapFrom(src => src.Sessions != null ? src.Sessions.Count : 0))
            ;
        
        CreateMap<EventType, EventConfigurationViewModel>().ReverseMap();
        CreateMap<EventSession, EventSessionViewModel>().ReverseMap();
    }
   
    public int Order => 1;
}