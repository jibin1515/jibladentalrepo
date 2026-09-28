using Application.Helpers;
using Application.Models.Common;
using Microsoft.AspNetCore.Http;

namespace Application.Models
{
	public class InsuranceDto : OrderableDto
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
		public string? ImagePath { get; set; }
		public string? ImageAlt { get; set; }
		public IFormFile? Image { get; set; }
		public string? Url { get; set; }
	}

}
