using System.Linq.Expressions;
using ChurchManager.Domain.Shared;
using CodeBoss.Extensions;

namespace ChurchManager.Domain.Features.Events.Specifications;

public static class ExpressionExtensions
{
    public static Expression<Func<Event, EventViewModel>> SelectEventWithDetails = x =>
        new EventViewModel
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            PhotoUrl = x.PhotoUrl,
            EventTypeId = x.EventTypeId,
            EventTypeName = x.EventType != null ? x.EventType.Name : null,
            ContactPhone = x.ContactPhone,
            ContactEmail = x.ContactEmail,
            ContactPerson =  x.ContactPerson != null ? new PersonViewModelBasic
            {
                PersonId = x.ContactPersonId,
                FirstName = x.ContactPerson.FullName!.FirstName!,
                LastName = x.ContactPerson.FullName.LastName!,
                Email = x.ContactPerson.Email.Address,
                PhotoUrl = x.ContactPerson.PhotoUrl,
            } : null,
            ChurchReference = new ChurchReference
            {
                ChurchId = x.ChurchId,
                ChurchGroupId = x.ChurchGroupId,
                ChurchName =  x.Church != null ? x.Church.Name : null,
                ChurchGroupName = x.Church != null && x.Church.ChurchGroup != null ? x.Church.ChurchGroup.Name : null
            },
            Location = x.Location,
            ChildCareGroup = x.ChildCareGroupId != null ? new GroupReference
            {
                GroupTypeId = x.ChildCareGroup!.GroupTypeId,
                GroupId = x.ChildCareGroupId,
                GroupTypeName = x.ChildCareGroup!.GroupType!.Name!,
                GroupName = x.ChildCareGroup.Name,
            } : null,
            EventRegistrationGroup = x.EventRegistrationGroupId != null ? new GroupReference
            {
                GroupTypeId = x.EventRegistrationGroup!.GroupTypeId,
                GroupId = x.EventRegistrationGroupId,
                GroupTypeName = x.EventRegistrationGroup!.GroupType!.Name,
                GroupName = x.EventRegistrationGroup.Name,
            } : null,
            Configuration = new EventConfigurationViewModel
            {
                OnlineSupport = x.EventType !=null ? x.EventType.OnlineSupport : OnlineSupport.Unknown.Value,
                RequiresRegistration = x.EventType!.RequiresRegistration,
                AllowFamilyRegistration = x.EventType.AllowFamilyRegistration,
                AllowNonFamilyRegistration = x.EventType.AllowNonFamilyRegistration,
                RequiresChildInfo = x.EventType.RequiresChildInfo,
                TakesAttendance = x.EventType.TakesAttendance,
                HasChildCare = x.EventType.ChildCare != null ? x.EventType.ChildCare.HasChildCare : null,
                MinChildAge = x.EventType.ChildCare != null ? x.EventType.ChildCare.MinChildAge : null,
                MaxChildAge =  x.EventType.ChildCare != null ? x.EventType.ChildCare.MaxChildAge : null,
            },
            ApprovalStatus = x.ApprovalStatus.Value,
            NumberOfSessions = x.Sessions != null ? x.Sessions.Count : 0,
            Sessions = x.Sessions != null ? x.Sessions.Select(x => new EventSessionViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                SessionOrder = x.SessionOrder,
                StartDate = x.SessionStartDateTime().StartDate,
                StartTime = x.SessionStartDateTime().StartTime.HasValue ? x.SessionStartDateTime().StartTime!.Value.ToString(@"hh\:mm") : null,
                EndDate = x.SessionEndDateTime().EndDate,
                EndTime = x.SessionEndDateTime().EndTime.HasValue ? x.SessionEndDateTime().EndTime!.Value.ToString(@"hh\:mm") : null,
                Location = x.Location,
                OnlineSupport = x.OnlineSupport,
                OnlineMeetingUrl = x.OnlineMeetingUrl,
                AttendanceRequired = x.AttendanceRequired,
            }) : Array.Empty<EventSessionViewModel>(),
            
            RegistrationStartDate = x.RegistrationDates != null ? x.RegistrationDates.StartDate : null,
            RegistrationEndDate = x.RegistrationDates != null ? x.RegistrationDates.EndDate : null,
        };
    
    
     public static Expression<Func<bool, EventType, EventTypeViewModel>> SelectEventTypeWithDetails = (includeDetails,x) =>
        new EventTypeViewModel
        {
            Id = x.Id,
            Name = x.Name!,
            Description = x.Description,
            AgeClassification = x.AgeClassification!.Value,
            OnlineSupport = x.OnlineSupport.Value,
            RequiresRegistration = x.RequiresRegistration,
            AllowFamilyRegistration = x.AllowFamilyRegistration,
            AllowNonFamilyRegistration = x.AllowNonFamilyRegistration,
            RequiresChildInfo = x.RequiresChildInfo,
            TakesAttendance = x.TakesAttendance,
            HasChildCare = x.ChildCare != null ? x.ChildCare.HasChildCare : null,
            MinChildAge = x.ChildCare != null ? x.ChildCare.MinChildAge : null,
            MaxChildAge = x.ChildCare != null ? x.ChildCare.MaxChildAge : null,
            IconCssClass = x.IconCssClass,
            DefaultGroupTypeId = x.DefaultGroupTypeId,
            GroupTypeName = x.DefaultGroupType != null ? x.DefaultGroupType.Name : null,
            IsSystem = x.IsSystem,
            
            Events = includeDetails && x.Events!= null? x.Events.Select(x => new EventViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                PhotoUrl = x.PhotoUrl,
                EventTypeId = x.EventTypeId,
                EventTypeName = x.EventType != null ? x.EventType.Name : null,
                ContactPhone = x.ContactPhone,
                ContactEmail = x.ContactEmail,
                ChurchReference = new ChurchReference
                {
                    ChurchId = x.ChurchId,
                    ChurchGroupId = x.ChurchGroupId,
                    ChurchName = x.Church != null ? x.Church.Name : null,
                    ChurchGroupName =   x.Church != null && x.Church.ChurchGroup != null ? x.Church.ChurchGroup.Name : null,
                },
                Location = x.Location,
                ApprovalStatus = x.ApprovalStatus.Value,
                RegistrationStartDate = x.RegistrationDates != null ? x.RegistrationDates.StartDate : null,
                RegistrationEndDate = x.RegistrationDates != null ? x.RegistrationDates.EndDate : null,
            }) : Array.Empty<EventViewModel>()
        };
}