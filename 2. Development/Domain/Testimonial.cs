using Domain.Common;

namespace Domain
{
    public class Testimonial : OrderableBaseEntity
    {
        public string? Name { get; set; }
        public string? Message { get; set; }
        public string? ImagePath { get; set; }
        public string? ImageAlt { get; set; }
    }
}
