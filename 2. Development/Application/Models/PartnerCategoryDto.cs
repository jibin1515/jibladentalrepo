using Application.Helpers;
using Application.Models.Common;

namespace Application.Models
{
    public class PartnerCategoryDto : OrderableDto
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
    }
}
