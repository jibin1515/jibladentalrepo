using Domain.Common;

namespace Domain
{
    public class Partner : OrderableBaseEntity
    {
        public long PartnerCategoryId { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public string? ShortBody { get; set; }
        public string? ImagePath { get; set; }
        public string? ImageAlt { get; set; }
		public string? LogoPath { get; set; }
        public string? LogoAlt { get; set; }
		public string? PageName { get; set; }
        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }
    }
}
