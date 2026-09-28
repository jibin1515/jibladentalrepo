using Application.Models.Common;
using Microsoft.AspNetCore.Http;

namespace Application.Models
{
    public class LoyaltyImageDto : OrderableDto
    {
        public long LoyaltyId { get; set; }
        public string? ImagePath { get; set; }
        public string? ImageAlt { get; set; }
        public IFormFile? Image { get; set; }
    }
}
