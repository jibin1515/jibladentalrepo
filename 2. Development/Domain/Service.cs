using Domain.Common;

namespace Domain
{
    public class Service : OrderableBaseEntity
    {
        public string? Title { get; set; }
        public string? Body { get; set; }
        public string? ShortBody { get; set; }
        public string? ImagePath { get; set; }
        public string? ImageAlt { get; set; }
        public string? HomeImagePath { get; set; }
        public string? HomeImageAlt { get; set; }
        public string? IconPath { get; set; }
        public string? IconAlt { get; set; }
        public string? BannerTitle { get; set; }
        public string? BannerImagePath { get; set; }
        public bool ShowOnHomePage { get; set; }
        public string? PageName { get; set; }
        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }
    }
}
