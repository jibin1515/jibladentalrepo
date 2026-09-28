using Domain.Common;

namespace Domain
{
    public class LoyaltyImage : OrderableBaseEntity
    {
        public long LoyaltyId { get; set; }
        public string? ImagePath { get; set; }
        public string? ImageAlt { get; set; }

	}
}
