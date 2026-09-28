using Domain.Common;

namespace Domain
{
    public class Faq : OrderableBaseEntity
    {
        public string? Title { get; set; }
        public string? Body { get; set; }
    }
}
