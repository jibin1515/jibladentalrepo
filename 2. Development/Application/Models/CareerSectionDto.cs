using Application.Helpers;
using Application.Models.Common;

namespace Application.Models
{
	public class CareerSectionDto : BaseDto
    {
		public string? Title { get; set; }
		public string? FormTitle { get; set; }
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
		public string? FormTitleEnglish
		{
			get => Localization.GetEnglish(FormTitle);
			set => FormTitle = Localization.Serialize(value, FormTitleArabic);
		}
		public string? FormTitleArabic
		{
			get => Localization.GetArabic(FormTitle);
			set => FormTitle = Localization.Serialize(FormTitleEnglish, value);
		}
	}
}
