using Application.Helpers;
using Application.Models.Common;

namespace Application.Models
{
    public class TeamSectionDto : BaseDto
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
    }
}
