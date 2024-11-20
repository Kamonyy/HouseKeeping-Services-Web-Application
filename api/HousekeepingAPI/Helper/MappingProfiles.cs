using AutoMapper;
using HousekeepingAPI.Dto;
using HousekeepingAPI.Models;

namespace HousekeepingAPI.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<Models.Service, ServiceDto>();
        }
    }
}
