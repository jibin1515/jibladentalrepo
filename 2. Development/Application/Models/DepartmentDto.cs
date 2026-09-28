using Application.Helpers;
using Application.Models.Common;

namespace Application.Models
{
    public class DepartmentDto : OrderableDto
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
