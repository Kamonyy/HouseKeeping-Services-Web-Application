using AutoMapper;
using HousekeepingAPI.Dto.Service;
using HousekeepingAPI.Dto.ServiceSubCategory;
using HousekeepingAPI.Dto.SubCategory;
using HousekeepingAPI.Dto.Comment;
using HousekeepingAPI.Models;

namespace HousekeepingAPI.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Models.Service, ServiceListDto>()
                .ForMember(dest => dest.ProviderUsername, opt => opt.MapFrom(src => src.Provider.UserName))
                .ForMember(dest => dest.SubCategories, opt => opt.MapFrom(src => src.ServiceSubCategory.Select(ssc =>
                    new ServiceSubCategoryDto
                    {
                        SubCategoryId = ssc.SubCategoryId,
                        SubCategoryName = ssc.SubCategory.Name,
                        CategoryName = ssc.SubCategory.Category.Name,
                        Id = ssc.SubCategory.Id,
                        Name = ssc.SubCategory.Name,
                        CategoryId = ssc.SubCategory.CategoryId
                    })));

            CreateMap<Models.Service, ServiceDetailsDto>()
                .ForMember(dest => dest.ProviderId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.ProviderUsername, opt => opt.MapFrom(src => src.Provider.UserName))
                .ForMember(dest => dest.SubCategories, opt => opt.MapFrom(src => src.ServiceSubCategory.Select(ssc =>
                    new ServiceSubCategoryDto
                    {
                        SubCategoryId = ssc.SubCategoryId,
                        SubCategoryName = ssc.SubCategory.Name,
                        CategoryName = ssc.SubCategory.Category.Name,
                        Id = ssc.SubCategory.Id,
                        Name = ssc.SubCategory.Name,
                        CategoryId = ssc.SubCategory.CategoryId
                    })));

            CreateMap<Models.Service, CreateServiceDto>().ReverseMap();

            // SubCategory Mappings
            CreateMap<SubCategory, SubCategoryDto>()
                .ForMember(dto => dto.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<CreateSubCategoryDto, SubCategory>();
            CreateMap<CreateServiceDto, Models.Service>()
               .ForMember(dest => dest.ServiceSubCategory, opt => opt.Ignore());

            // Comment Mappings
            CreateMap<CommentCreateDto, Models.Comment>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<CommentUpdateDto, Models.Comment>();
        }
    }
}