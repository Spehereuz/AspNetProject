using ASP.NET_Project.Data.Entities;
using ASP.NET_Project.Models.Category;
using AutoMapper;

namespace ASP.NET_Project.Mapper
{
    public class CategoryMapper : Profile
    {
        public CategoryMapper()
        {
            CreateMap<CategoryEntity, CategoryItemViewModel>()
                .ForMember(x => x.Image, opt => opt.MapFrom(x => x.ImageUrl));
        }
    }
}
