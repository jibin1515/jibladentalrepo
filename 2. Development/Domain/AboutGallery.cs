using Domain.Common;

namespace Domain
{
    public class AboutGallery : OrderableBaseEntity
    {
        public string? ImagePath { get; set; }
        public string? ImageAlt { get; set; }
    }
}
