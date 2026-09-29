using Application.Helpers;
using Application.Models.Common;
using Microsoft.AspNetCore.Http;

namespace Application.Models;

public class PageSettingsDto : OrderableDto
{
    public string? Name { get; set; }
    public string? ParentName { get; set; }
    public long? EntityId { get; set; }
    public string? Title { get; set; }
    public string? BannerTitle { get; set; }
    public string? BannerSubTitle { get; set; }
    public string? BannerImagePath { get; set; }
    public string? BannerImageAlt { get; set; }
    public IFormFile? BannerImage { get; set; }
	public string? BannerArabicImagePath { get; set; }
	public IFormFile? BannerArabicImage { get; set; }
	public bool ShowOnFooter { get; set; }
    public string? SeoTitle { get; set; }
    public string? SeoDescription { get; set; }

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