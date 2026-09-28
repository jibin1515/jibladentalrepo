using Application.Helpers;
using Application.Models.Common;
using Microsoft.AspNetCore.Http;

namespace Application.Models
{
    public class NewsDto : OrderableDto
    {
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
        public string? PostDate { get; set; }
        public string? PostDateEnglish
        {
            get => Localization.GetEnglish(PostDate);
            set => PostDate = Localization.Serialize(value, PostDateArabic);
        }
        public string? PostDateArabic
        {
            get => Localization.GetArabic(PostDate);
            set => PostDate = Localization.Serialize(PostDateEnglish, value);
        }
        public string? HomeImagePath { get; set; }
        public string? HomeImageAlt { get; set; }
        public IFormFile? HomeImage { get; set; }
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
        public string? BannerTitle { get; set; }
        public string? BannerTitleEnglish
        {
            get => Localization.GetEnglish(BannerTitle);
            set => BannerTitle = Localization.Serialize(value, BannerTitleArabic);
        }
        public string? BannerTitleArabic
        {
            get => Localization.GetArabic(BannerTitle);
            set => BannerTitle = Localization.Serialize(BannerTitleEnglish, value);
        }
        public string? BannerImagePath { get; set; }
        public IFormFile? BannerImage { get; set; }
    }
}
