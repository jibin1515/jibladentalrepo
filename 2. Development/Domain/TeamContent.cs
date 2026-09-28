using Domain.Common;

namespace Domain
{
    public class TeamContent : OrderableBaseEntity
    {
        public long TeamId { get; set; }
        public string? Type { get; set; }//social,service,qualification,gallery
        public string? Name { get; set; }
        public string? Url { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public string? ImagePath { get; set; }
        public string? ImageAlt { get; set; }
    }
}
