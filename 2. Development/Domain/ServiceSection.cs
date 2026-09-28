using Domain.Common;

namespace Domain
{
    public class ServiceSection : BaseEntity
    {
        public string? Tagline { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
    }
}
