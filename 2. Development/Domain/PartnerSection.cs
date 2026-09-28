using Domain.Common;

namespace Domain
{
    public class PartnerSection : BaseEntity
    {
        public string? Title { get; set; }
        public string? Body { get; set; }
        public string? HomeTagline { get; set; }
        public string? HomeTitle { get; set; }
        public string? HomeBody { get; set; }
    }
}
