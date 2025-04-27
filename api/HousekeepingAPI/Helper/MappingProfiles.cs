using AutoMapper;
using HousekeepingAPI.Dto.Service;
using HousekeepingAPI.Dto.ServiceSubCategory;
using HousekeepingAPI.Dto.SubCategory;
using HousekeepingAPI.Models;

namespace HousekeepingAPI.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Models.Service, ServiceListDto>()
                .ForMember(dest => dest.ProviderUsername, opt => opt.MapFrom(src => src.Provider.UserName));

            CreateMap<Models.Service, ServiceDetailsDto>()
                .ForMember(dest => dest.ProviderId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.ProviderUsername, opt => opt.MapFrom(src => src.Provider.UserName))
                .ForMember(dest => dest.SubCategories, opt => opt.MapFrom(src => src.ServiceSubCategory.Select(ssc =>
                    new ServiceSubCategoryDto
                    {
                        SubCategoryId = ssc.SubCategoryId,
                        SubCategoryName = ssc.SubCategory.Name,
                        CategoryName = ssc.SubCategory.Category.Name
                    })));

            CreateMap<Models.Service, CreateServiceDto>().ReverseMap();

            // SubCategory Mappings
            CreateMap<SubCategory, SubCategoryDto>()
                .ForMember(dto => dto.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<CreateSubCategoryDto, SubCategory>();
            CreateMap<CreateServiceDto, Models.Service>()
               .ForMember(dest => dest.ServiceSubCategory, opt => opt.Ignore());
        }
    }
}