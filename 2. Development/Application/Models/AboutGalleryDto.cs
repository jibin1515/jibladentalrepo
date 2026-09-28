using Application.Models.Common;
using Microsoft.AspNetCore.Http;

namespace Application.Models
{
    public class AboutGalleryDto : OrderableDto
    {
        public string? ImagePath { get; set; }
        public string? ImageAlt { get; set; }
        public IFormFile? Image { get; set; }
    }
}
