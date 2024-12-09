using AutoMapper;
using HousekeepingAPI.Dto;
using HousekeepingAPI.Dto.SubCategory;
using HousekeepingAPI.Models;

namespace HousekeepingAPI.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<Models.Service, ServiceDto>();
            CreateMap<SubCategory, SubCategoryDto>()
                .ForMember(dto => dto.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

        }
    }
}
