using HousekeepingAPI.Dto.ServiceSubCategory;
using System;
using System.Collections.Generic;

namespace HousekeepingAPI.Dto.Service
{
    public class ServiceListDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ProviderUsername { get; set; } = string.Empty;
        public decimal EstimatedPrice { get; set; }
        public string ContactPhone { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public bool IsApproved { get; set; }
        public string ProviderFirstName { get; set; } = string.Empty;
        public string ProviderLastName { get; set; } = string.Empty;
        
        // Add missing SubCategories collection for frontend
        public List<ServiceSubCategoryDto> SubCategories { get; set; } = new List<ServiceSubCategoryDto>();
        
        // Rating properties
        public double AverageRating { get; set; } = 0;
        public int RatingCount { get; set; } = 0;
    }
}