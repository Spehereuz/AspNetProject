using ASP.NET_Project.Data.Entities;
using ASP.NET_Project.Models.Seeder;
using AutoMapper;

namespace ASP.NET_Project.Mapper
{
    public class SeederMapper : Profile
    {
        public SeederMapper()
        {
            CreateMap<SeederCategoryModel, CategoryEntity>()
                .ForMember(x => x.ImageUrl, opt => opt.MapFrom(x => x.Image));
        }
    }
}
