using Application.Helpers;
using Application.Models.Common;
using Microsoft.AspNetCore.Http;

namespace Application.Models
{
    public class PartnerDto : OrderableDto
    {
        public long PartnerCategoryId { get; set; }
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
        public string? ShortBody { get; set; }
        public string? ShortBodyEnglish
        {
            get => Localization.GetEnglish(ShortBody);
            set => ShortBody = Localization.Serialize(value, ShortBodyArabic);
        }
        public string? ShortBodyArabic
        {
            get => Localization.GetArabic(ShortBody);
            set => ShortBody = Localization.Serialize(ShortBodyEnglish, value);
        }
        public string? ImagePath { get; set; }
        public string? ImageAlt { get; set; }
        public IFormFile? Image { get; set; }
        public string? LogoPath { get; set; }
        public string? LogoAlt { get; set; }
        public IFormFile? Logo { get; set; }
        public string? PageName { get; set; }
        public string? SeoTitle { get; set; }
        public string? SeoTitleEnglish
        {
            get => Localization.GetEnglish(SeoTitle);
            set => SeoTitle = Localization.Serialize(value, SeoTitleArabic);
        }
        public string? SeoTitleArabic
        {
            get => Localization.GetArabic(SeoTitle);
            set => SeoTitle = Localization.Serialize(SeoTitleEnglish, value);
        }
        public string? SeoDescription { get; set; }
        public string? SeoDescriptionEnglish
        {
            get => Localization.GetEnglish(SeoDescription);
            set => SeoDescription = Localization.Serialize(value, SeoDescriptionArabic);
        }
        public string? SeoDescriptionArabic
        {
            get => Localization.GetArabic(SeoDescription);
            set => SeoDescription = Localization.Serialize(SeoDescriptionEnglish, value);
        }
    }
}
