﻿namespace HousekeepingAPI.Dto.Service
{
    public class CreateServiceDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal EstimatedPrice { get; set; }
        public string ContactPhone { get; set; } = string.Empty;

        public List<int> SubCategoryIds { get; set; } = new();
    }
}
