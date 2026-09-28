using Application.Models.Common;
using Microsoft.AspNetCore.Http;

namespace Application.Models
{
    public class NewsImageDto : OrderableDto
    {
        public long NewsId { get; set; }
        public string? ImagePath { get; set; }
        public string? ImageAlt { get; set; }
        public IFormFile? Image { get; set; }
    }
}
