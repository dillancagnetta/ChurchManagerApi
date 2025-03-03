using AutoMapper;
using ChurchManager.Application.ViewModels;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.Communications;
using ChurchManager.Domain.Features.People;
using ChurchManager.Domain.Shared;
using ChurchManager.Infrastructure.Mapper;
using Convey.CQRS.Queries;
using GroupMemberViewModel = ChurchManager.Domain.Shared.GroupMemberViewModel;

namespace ChurchManager.Features.People.Infrastructure.Mapper
{
    public class PeopleMappingProfile : Profile, IAutoMapperProfile
    {
        public PeopleMappingProfile()
        {
            CreateMap<Person, PersonViewModel>()
                .ForMember(d => d.PersonId, opt => opt.MapFrom(src => src.Id))
                ;

            CreateMap<Person, PersonViewModelBasic>()
                .ForMember(d => d.PersonId, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.Title, opt => opt.MapFrom(src => src.FullName.Title))
                .ForMember(d => d.FirstName, opt => opt.MapFrom(src => src.FullName.FirstName))
                .ForMember(d => d.LastName, opt => opt.MapFrom(src => src.FullName.LastName))
                .ForMember(d => d.Email, opt => 
                    opt.MapFrom(src => src.Email != null ? src.Email.Address : null))
                .ForMember(d => d.Age, opt => 
                    opt.MapFrom(src => src.BirthDate != null ? src.BirthDate.Age : null))
                .ForMember(d => d.AgeClassification,
                    o => o.MapFrom(src =>
                        src.AgeClassification == null ? AgeClassification.Unknown.Value : src.AgeClassification.Value))
                .ForMember(d => d.PhotoUrl,
                    opt => opt.MapFrom(src => src.PhotoUrl))
                ;
            
            CreateMap<PersonViewModelBasic, Person>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.PersonId))
                .ForMember(d => d.FullName, opt => opt.MapFrom(src => new FullName
                {
                    Title = src.Title,
                    FirstName = src.FirstName,
                    LastName = src.LastName
                }))
                .ForMember(d => d.Email, opt => opt.MapFrom(src => 
                    !string.IsNullOrEmpty(src.Email) ? new Email() {Address = src.Email} : null))
                .ForMember(d => d.AgeClassification, opt => opt.MapFrom(src => 
                    new AgeClassification(src.AgeClassification)))
                .ForMember(d => d.PhotoUrl, opt => opt.MapFrom(src => src.PhotoUrl))
                //.ForAllMembers(opt => opt.Ignore())
                ;

            CreateMap<Person, GroupMemberViewModel>()
                .ForMember(d => d.FirstName,
                    opt => opt.MapFrom(src => src.FullName.FirstName))
                .ForMember(d => d.LastName,
                    opt => opt.MapFrom(src => src.FullName.LastName))
                .ForMember(d => d.MiddleName,
                    opt => opt.MapFrom(src => src.FullName.MiddleName))
                .ForMember(d => d.PhotoUrl,
                    opt => opt.MapFrom(src => src.PhotoUrl))
                .ForMember(d => d.PersonId,
                    opt => opt.MapFrom(src => src.Id))
                ;

            /*
            CreateMap<Person, PersonViewModelShared>()
                .ForMember(d => d.FirstName,
                    opt => opt.MapFrom(src => src.FullName.FirstName))
                .ForMember(d => d.LastName,
                    opt => opt.MapFrom(src => src.FullName.LastName))
                .ForMember(d => d.PhotoUrl,
                    opt => opt.MapFrom(src => src.PhotoUrl))
                .ForMember(d => d.PersonId,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.AgeClassification,
                    o => o.MapFrom(src =>
                        src.AgeClassification == null ? AgeClassification.Unknown.Value : src.AgeClassification.Value))*/
                ;

            CreateMap<PagedResult<Person>, PagedResult<PersonViewModel>>()
                .ForMember(d => d.Items,
                    opt => opt.MapFrom(src => src.Items));


            CreateMap<BirthDate, BirthDateViewModel>().ReverseMap();
            
            CreateMap<Person, PersonViewModel>()
                .ForMember(d => d.PersonId, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.ConnectionStatus,
                    o => o.MapFrom(src =>
                        src.ConnectionStatus == null ? ConnectionStatus.Unknown.Value : src.ConnectionStatus.Value))
                .ForMember(d => d.Gender,
                    o => o.MapFrom(src => src.Gender == null ? Gender.Unknown.Value : src.Gender.Value))
                .ForMember(d => d.AgeClassification,
                    o => o.MapFrom(src =>
                        src.AgeClassification == null ? AgeClassification.Unknown.Value : src.AgeClassification.Value))
                .ForMember(d => d.CommunicationPreference,
                    o => o.MapFrom(src =>
                        src.CommunicationPreference == null
                            ? CommunicationType.None.Value
                            : src.CommunicationPreference.Value))
                // Gets the persons family members excluding them
                .ForMember(d => d.FamilyMembers, o => o.MapFrom(src
                    => src.Family == null
                        ? new List<PersonViewModelBasic>(0)
                        : src.Family.FamilyMembers
                            .Where(x => x.Id != src.Id)
                            .Select(x => new PersonViewModelBasic
                            {
                                PersonId = x.Id,
                                FirstName = x.FullName.FirstName,
                                LastName = x.FullName.LastName,
                                AgeClassification = x.AgeClassification,
                                // BirthDate = x.BirthDate,  // AUTOMAPPED
                                Gender = x.Gender,
                                PhotoUrl = x.PhotoUrl
                            })
                            .ToList()))
                ;
        }
        public int Order => 1;
    }
}