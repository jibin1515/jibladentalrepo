using Domain.Common;

namespace Domain
{
    public class VideoHub : OrderableBaseEntity
    {
        public string? Title { get; set; }
        public string? Body { get; set; }
        public string? ImagePath { get; set; }
        public string? ImageAlt { get; set; }
        public string? VideoUrl { get; set; }
    }
}
