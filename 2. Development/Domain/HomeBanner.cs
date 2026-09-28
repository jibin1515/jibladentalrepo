using Domain.Common;

namespace Domain
{
    public class HomeBanner : OrderableBaseEntity
    {
        public string? Tagline { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public string? ImagePath { get; set; }
        public string? ImageAlt { get; set; }
		public string? ImageArabicPath { get; set; }
		public string? ImageArabicAlt { get; set; }
		public string? ButtonTitle { get; set; }
        public string? ButtonUrl { get; set; }
    }
}
