using Application.Helpers;
using Application.Models.Common;

namespace Application.Models
{
    public class PartnerSectionDto : BaseDto
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
        public string? HomeTagline { get; set; }
        public string? HomeTaglineEnglish
        {
            get => Localization.GetEnglish(HomeTagline);
            set => HomeTagline = Localization.Serialize(value, HomeTaglineArabic);
        }
        public string? HomeTaglineArabic
        {
            get => Localization.GetArabic(HomeTagline);
            set => HomeTagline = Localization.Serialize(HomeTaglineEnglish, value);
        }
        public string? HomeTitle { get; set; }
        public string? HomeTitleEnglish
        {
            get => Localization.GetEnglish(HomeTitle);
            set => HomeTitle = Localization.Serialize(value, HomeTitleArabic);
        }
        public string? HomeTitleArabic
        {
            get => Localization.GetArabic(HomeTitle);
            set => HomeTitle = Localization.Serialize(HomeTitleEnglish, value);
        }
        public string? HomeBody { get; set; }
        public string? HomeBodyEnglish
        {
            get => Localization.GetEnglish(HomeBody);
            set => HomeBody = Localization.Serialize(value, HomeBodyArabic);
        }
        public string? HomeBodyArabic
        {
            get => Localization.GetArabic(HomeBody);
            set => HomeBody = Localization.Serialize(HomeBodyEnglish, value);
        }
    }

}
