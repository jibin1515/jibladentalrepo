using Domain.Common;

namespace Domain
{
    public class Career : OrderableBaseEntity
    {
        public string? Title { get; set; }
        public string? Body { get; set; }
        public string? ShortBody { get; set; }
        public string? Title2 { get; set; }
        public string? Body2 { get; set; }
        public string? Title3 { get; set; }
        public string? Body3 { get; set; }
        public string? JobType { get; set; }
        public string? Email { get; set; }
        public long? Vacancy { get; set; }
		public string? IconPath { get; set; }
		public string? IconAlt { get; set; }
		public string? PageName { get; set; }
        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }
        public string? BannerTitle { get; set; }
        public string? BannerImagePath { get; set; }
    }
}
