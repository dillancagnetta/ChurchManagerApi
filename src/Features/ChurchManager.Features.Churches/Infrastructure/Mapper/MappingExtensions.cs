﻿using ChurchManager.Domain.Features.Churches;
using ChurchManager.Infrastructure.Mapper;
using ChurchViewModel = ChurchManager.Domain.Shared.ChurchViewModel;

namespace ChurchManager.Features.Churches.Infrastructure.Mapper
{
    public static class MappingExtensions
    {
        public static ChurchViewModel ToModel(this Church entity)
        {
            return entity.MapTo<Church, ChurchViewModel>();
        }

        public static IQueryable<ChurchViewModel> ToProjection(this IQueryable<Church> source)
        {
            return source.MapTo<Church, ChurchViewModel>();
        }
    }
}
