using Application.Helpers;
using Application.Models.Common;
using Microsoft.AspNetCore.Http;

namespace Application.Models
{
    public class HomeBannerDto : OrderableDto
    {
        public string? Tagline { get; set; }
        public string? TaglineEnglish
        {
            get => Localization.GetEnglish(Tagline);
            set => Tagline = Localization.Serialize(value, TaglineArabic);
        }
        public string? TaglineArabic
        {
            get => Localization.GetArabic(Tagline);
            set => Tagline = Localization.Serialize(TaglineEnglish, value);
        }
        public string? Title { get; set; }
        public string? TitleEnglish
        {
            get => Localization.GetEnglish(Title);
            set => Title = Localization.Serialize(value, TitleArabic);
        }
        public string? TitleArabic
        {
            get => Localization.GetArabic(Title);
            set => Title = Localization.Serialize(TitleEnglish, value);
        }
        public string? Body { get; set; }
        public string? BodyEnglish
        {
            get => Localization.GetEnglish(Body);
            set => Body = Localization.Serialize(value, BodyArabic);
        }
        public string? BodyArabic
        {
            get => Localization.GetArabic(Body);
            set => Body = Localization.Serialize(BodyEnglish, value);
        }
        public string? ImagePath { get; set; }
        public string? ImageAlt { get; set; }
        public IFormFile? Image { get; set; }
		public string? ImageArabicPath { get; set; }
		public string? ImageArabicAlt { get; set; }
		public IFormFile? ImageArabic { get; set; }
		public string? ButtonTitle { get; set; }
        public string? ButtonTitleEnglish
        {
            get => Localization.GetEnglish(ButtonTitle);
            set => ButtonTitle = Localization.Serialize(value, ButtonTitleArabic);
        }
        public string? ButtonTitleArabic
        {
            get => Localization.GetArabic(ButtonTitle);
            set => ButtonTitle = Localization.Serialize(ButtonTitleEnglish, value);
        }
        public string? ButtonUrl { get; set; }
    }
}
