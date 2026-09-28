using Domain.Common;

namespace Domain
{
    public class Policy : OrderableBaseEntity
    {
        public string? Title { get; set; }
        public string? Body { get; set; }
    }
}
