using AutoMapper;
using ChurchManager.Domain.Features.Events;
using ChurchManager.Features.Events.Commands;
using ChurchManager.Infrastructure.Mapper;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Features.Events.Infrastructure.Mapper;

public class EventsMappingProfile: Profile, IAutoMapperProfile
{
    public EventsMappingProfile()
    {
        CreateMap<EventViewModel, Event>();

        CreateMap<Event, EventViewModel>()
            .ForMember(d => d.ChurchReference, opt => opt.MapFrom(src => new ChurchReference
            {
                ChurchId = src.ChurchId,
                ChurchName = src.Church != null ? src.Church.Name : null,
                ChurchGroupId = src.ChurchGroupId,
                ChurchGroupName = src.Church != null && src.Church.ChurchGroup != null ? src.Church.ChurchGroup.Name : null
            }))
            .ForMember(d => d.EventRegistrationGroup, opt => opt.MapFrom(src => new GroupReference
            {
                GroupId = src.EventRegistrationGroupId,
                GroupName = src.EventRegistrationGroup != null ? src.EventRegistrationGroup.Name : null,
                GroupTypeId = src.EventRegistrationGroup != null ? src.EventRegistrationGroup.GroupTypeId : null,
                GroupTypeName = src.EventRegistrationGroup != null && src.EventRegistrationGroup.GroupType != null ? src.EventRegistrationGroup.GroupType.Name : null
            }))
            .ForMember(d => d.ChildCareGroup, opt => opt.MapFrom(src => new GroupReference
            {
                GroupId = src.ChildCareGroupId,
                GroupName = src.ChildCareGroup != null ? src.ChildCareGroup.Name : null,
                GroupTypeId = src.ChildCareGroup != null ? src.ChildCareGroup.GroupTypeId : null,
                GroupTypeName = src.ChildCareGroup != null && src.ChildCareGroup.GroupType != null ? src.ChildCareGroup.GroupType.Name : null
            }))
            .ForMember(d => d.EventTypeName, opt =>
                opt.MapFrom(src => src.EventType != null ? src.EventType.Name : null))
            
            // Session Summary
            .ForMember(d => d.NumberOfSessions, opt =>
                opt.MapFrom(src => src.Sessions != null ? src.Sessions.Count : 0))
            .ForMember(d => d.StartDate, opt =>
                opt.MapFrom(src => src.FirstSessionDateTime().StartDate))
            .ForMember(d => d.StartTime, opt =>
                opt.MapFrom(src => src.FirstSessionDateTime().StartTime))
            .ForMember(d => d.EndDate, opt =>
                opt.MapFrom(src => src.LastSessionDateTime().EndDate))
            .ForMember(d => d.StartTime, opt =>
                opt.MapFrom(src => src.LastSessionDateTime().EndTime))
            ;
        
        // For External Public API
        CreateMap<EventType, EventConfigurationViewModel>().ReverseMap();
        CreateMap<EventSession, EventSessionViewModel>()
            .ForMember(d => d.StartDate, opt =>
                opt.MapFrom(src => src.Schedule != null ? src.Schedule.StartDate.GetValueOrDefault() : (DateTime?) null))
            .ForMember(d => d.EndDate, opt =>
                opt.MapFrom(src => src.Schedule != null ? src.Schedule.EndDate.GetValueOrDefault() : (DateTime?) null))
            .ForMember(d => d.StartTime, opt =>
                opt.MapFrom(src => src.Schedule != null ? src.SessionStartDateTime().StartTime : null))
            .ForMember(d => d.EndTime, opt =>
                opt.MapFrom(src => src.Schedule != null ? src.SessionEndDateTime().EndTime : null))
            ;
        // For External Public API
        
        // For Internal API
        CreateMap<EventType, EventTypeViewModel>()
            .ForMember(d => d.HasChildCare, opt =>
                opt.MapFrom(src => src.ChildCare != null && src.ChildCare.HasChildCare))
            .ForMember(d => d.MinChildAge, opt =>
                opt.MapFrom(src => src.ChildCare != null && src.ChildCare.HasChildCare ? src.ChildCare.MinChildAge : null))
            .ForMember(d => d.MaxChildAge, opt =>
                opt.MapFrom(src => src.ChildCare != null && src.ChildCare.HasChildCare ? src.ChildCare.MinChildAge : null))
            ;

        // Event Types
        CreateMap<EditEventTypeCommand, EditEventTypeModel>().ReverseMap();
        CreateMap<EditEventTypeModel, EventType>();
        
        // Events
        CreateMap<EditEventViewModel, EditEventCommand>().ReverseMap();
        CreateMap<EditEventCommand, Event>()
            .ForMember(dest => dest.Sessions, opt => opt.Ignore()); // Ignore Sessions;
        CreateMap<EditEventViewModel, Event>();
        CreateMap<EventSessionViewModel, EventSession>()
            .ForMember(d => d.OnlineSupport, opt =>
                opt.MapFrom(src => new OnlineSupport(src.OnlineSupport)));
    }
   
    public int Order => 1;
}