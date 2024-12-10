using AutoMapper;
using HousekeepingAPI.Dto.Service;
using HousekeepingAPI.Dto.SubCategory;
using HousekeepingAPI.Models;

namespace HousekeepingAPI.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<Models.Service, ServiceDto>();

            CreateMap<Models.Service, CreateServiceDto>();

            CreateMap<CreateServiceDto, Models.Service>();

        CreateMap<SubCategory, SubCategoryDto>()
                .ForMember(dto => dto.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
            CreateMap<CreateSubCategoryDto, SubCategory>();
        }
    }
}
