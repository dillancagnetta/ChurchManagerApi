﻿using AutoMapper;
using ChurchManager.Application.ViewModels;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Infrastructure.Mapper;
using ChurchViewModel = ChurchManager.Domain.Shared.ChurchViewModel;

namespace ChurchManager.Features.Churches.Infrastructure.Mapper
{
    public class ChurchesMappingProfile : Profile, IAutoMapperProfile
    {
        public ChurchesMappingProfile()
        {
            CreateMap<Church, ChurchViewModel>().ReverseMap();
        }

        public int Order => 1;
    }
}