using Application.Helpers;
using Application.Models.Common;

namespace Application.Models
{
	public class InsuranceSectionDto : BaseDto
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
	}
}
