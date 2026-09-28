using Domain.Common;

namespace Domain
{
    public class NewsImage : OrderableBaseEntity
    {
        public long NewsId { get; set; }
        public string? ImagePath { get; set; }
        public string? ImageAlt { get; set; }

	}
}
