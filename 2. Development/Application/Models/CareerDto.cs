using Application.Helpers;
using Application.Models.Common;
using Microsoft.AspNetCore.Http;

namespace Application.Models
{
    public class CareerDto : OrderableDto
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
        public string? Title2 { get; set; }
        public string? Title2English
        {
            get => Localization.GetEnglish(Title2);
            set => Title2 = Localization.Serialize(value, Title2Arabic);
        }
        public string? Title2Arabic
        {
            get => Localization.GetArabic(Title2);
            set => Title2 = Localization.Serialize(Title2English, value);
        }
        public string? Body2 { get; set; }
        public string? Body2English
        {
            get => Localization.GetEnglish(Body2);
            set => Body2 = Localization.Serialize(value, Body2Arabic);
        }
        public string? Body2Arabic
        {
            get => Localization.GetArabic(Body2);
            set => Body2 = Localization.Serialize(Body2English, value);
        }
        public string? Title3 { get; set; }
        public string? Title3English
        {
            get => Localization.GetEnglish(Title3);
            set => Title3 = Localization.Serialize(value, Title3Arabic);
        }
        public string? Title3Arabic
        {
            get => Localization.GetArabic(Title3);
            set => Title3 = Localization.Serialize(Title3English, value);
        }
        public string? Body3 { get; set; }
        public string? Body3English
        {
            get => Localization.GetEnglish(Body3);
            set => Body3 = Localization.Serialize(value, Body3Arabic);
        }
        public string? Body3Arabic
        {
            get => Localization.GetArabic(Body3);
            set => Body3 = Localization.Serialize(Body3English, value);
        }
        public string? JobType { get; set; }
        public string? JobTypeEnglish
        {
            get => Localization.GetEnglish(JobType);
            set => JobType = Localization.Serialize(value, JobTypeArabic);
        }
        public string? JobTypeArabic
        {
            get => Localization.GetArabic(JobType);
            set => JobType = Localization.Serialize(JobTypeEnglish, value);
        }
        public string? Email { get; set; }
        public long? Vacancy { get; set; }
		public string? IconPath { get; set; }
        public IFormFile? Icon { get; set; }
		public string? IconAlt { get; set; }
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
