﻿using ASP.NET_Project.Data.Entities;
using ASP.NET_Project.Models.Category;
using AutoMapper;

namespace ASP.NET_Project.Mapper
{
    public class CategoryMapper : Profile
    {
        public CategoryMapper()
        {
            // Мапінг з CategoryEntity до CategoryItemViewModel
            CreateMap<CategoryEntity, CategoryItemViewModel>()
                .ForMember(x => x.Image, opt => opt.MapFrom(x => x.ImageUrl));

            // Мапінг з CategoryItemViewModel до CategoryEntity
            CreateMap<CategoryCreateViewModel, CategoryEntity>()
                .ForMember(x => x.ImageUrl, opt => opt.Ignore());

            // Мапінг з CategoryEditViewModel до CategoryEntity
            CreateMap<CategoryEditViewModel, CategoryEntity>();
        }
    }
}
