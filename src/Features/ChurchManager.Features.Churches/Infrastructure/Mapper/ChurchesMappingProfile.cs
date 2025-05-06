using AutoMapper;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Infrastructure.Mapper;
using ChurchViewModel = ChurchManager.Domain.Shared.ChurchViewModel;
using ChurchManager.Domain.Shared;

namespace ChurchManager.Features.Churches.Infrastructure.Mapper;

public class ChurchesMappingProfile : Profile, IAutoMapperProfile
{
    public ChurchesMappingProfile()
    {
        CreateMap<Church, ChurchViewModel>().ReverseMap();
            
        //CreateMap<ChurchGroup, ChurchGroupViewModel>().ReverseMap();
            
        CreateMap<ChurchGroupViewModel, ChurchGroup>()
            .ForMember(d => d.LeaderPersonId, opt =>
                opt.MapFrom(src => src.LeaderPerson != null ? (int?)src.LeaderPerson.PersonId : null ))
            // ignore so we dont try to create a new person as the LeaderPersonId is all we need 
            .ForMember(d => d.LeaderPerson, opt => opt.Ignore())
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            ;
        
        CreateMap<ChurchViewModel, Church>()
            .ForMember(d => d.LeaderPersonId, opt =>
                opt.MapFrom(src => src.LeaderPerson != null ? (int?)src.LeaderPerson.PersonId : null ))
            // ignore so we dont try to create a new person as the LeaderPersonId is all we need 
            .ForMember(d => d.LeaderPerson, opt => opt.Ignore())
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            ;

        CreateMap<ChurchGroup, ChurchGroupViewModel>();
        /*.ForMember(d => d.LeaderPerson, opt =>
            opt.MapFrom(src => src.LeaderPerson != null
                ? new PersonViewModelBasic
                {
                    PersonId = src.LeaderPerson.Id,
                    Title = src.LeaderPerson.FullName.Title,
                    FirstName = src.LeaderPerson.FullName.FirstName,
                    LastName = src.LeaderPerson.FullName.LastName,
                    Gender = src.LeaderPerson.Gender,
                    AgeClassification = src.LeaderPerson.AgeClassification,
                    PhotoUrl = src.LeaderPerson.PhotoUrl,
                    Email = src.LeaderPerson.Email != null ? src.LeaderPerson.Email.Address : null,
                    Age = src.LeaderPerson.BirthDate != null ? src.LeaderPerson.BirthDate.Age : null
                }
                : null
            ));*/

        // EDIT
        CreateMap<EditChurchModel, Church>().ReverseMap();
        CreateMap<EditChurchGroupModel, ChurchGroup>().ReverseMap();
        
        CreateMap<ChurchServiceTimeViewModel, ChurchServiceTime>().ReverseMap();
    }

    public int Order => 1;
}