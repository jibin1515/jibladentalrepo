using Application.Helpers;
using Application.Models.Common;
using Microsoft.AspNetCore.Http;

namespace Application.Models
{
    public class AboutDto : BaseDto
    {
        public string? CEOTagline { get; set; }
        public string? CEOTaglineEnglish
        {
            get => Localization.GetEnglish(CEOTagline);
            set => CEOTagline = Localization.Serialize(value, CEOTaglineArabic);
        }
        public string? CEOTaglineArabic
        {
            get => Localization.GetArabic(CEOTagline);
            set => CEOTagline = Localization.Serialize(CEOTaglineEnglish, value);
        }
        public string? CEOName { get; set; }
        public string? CEONameEnglish
        {
            get => Localization.GetEnglish(CEOName);
            set => CEOName = Localization.Serialize(value, CEONameArabic);
        }
        public string? CEONameArabic
        {
            get => Localization.GetArabic(CEOName);
            set => CEOName = Localization.Serialize(CEONameEnglish, value);
        }
        public string? CEOBody { get; set; }
        public string? CEOBodyEnglish
        {
            get => Localization.GetEnglish(CEOBody);
            set => CEOBody = Localization.Serialize(value, CEOBodyArabic);
        }
        public string? CEOBodyArabic
        {
            get => Localization.GetArabic(CEOBody);
            set => CEOBody = Localization.Serialize(CEOBodyEnglish, value);
        }
        public string? CEOImagePath { get; set; }
        public string? CEOImageAlt { get; set; }
        public IFormFile? CEOImage { get; set; }
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
        public string? ImagePath { get; set; }
        public string? ImageAlt { get; set; }
        public IFormFile? Image { get; set; }
        public string? VideoPath { get; set; }
        public IFormFile? Video { get; set; }
        public string? VisionMissionHeading { get; set; }
        public string? VisionMissionHeadingEnglish
        {
            get => Localization.GetEnglish(VisionMissionHeading);
            set => VisionMissionHeading = Localization.Serialize(value, VisionMissionHeadingArabic);
        }
        public string? VisionMissionHeadingArabic
        {
            get => Localization.GetArabic(VisionMissionHeading);
            set => VisionMissionHeading = Localization.Serialize(VisionMissionHeadingEnglish, value);
        }
        public string? VisionMissionBody { get; set; }
        public string? VisionMissionBodyEnglish
        {
            get => Localization.GetEnglish(VisionMissionBody);
            set => VisionMissionBody = Localization.Serialize(value, VisionMissionBodyArabic);
        }
        public string? VisionMissionBodyArabic
        {
            get => Localization.GetArabic(VisionMissionBody);
            set => VisionMissionBody = Localization.Serialize(VisionMissionBodyEnglish, value);
        }
        public string? VisionMissionImagePath { get; set; }
        public string? VisionMissionImageAlt { get; set; }
        public IFormFile? VisionMissionImage { get; set; }
        public string? VisionTitle { get; set; }
        public string? VisionTitleEnglish
        {
            get => Localization.GetEnglish(VisionTitle);
            set => VisionTitle = Localization.Serialize(value, VisionTitleArabic);
        }
        public string? VisionTitleArabic
        {
            get => Localization.GetArabic(VisionTitle);
            set => VisionTitle = Localization.Serialize(VisionTitleEnglish, value);
        }
        public string? VisionBody { get; set; }
        public string? VisionBodyEnglish
        {
            get => Localization.GetEnglish(VisionBody);
            set => VisionBody = Localization.Serialize(value, VisionBodyArabic);
        }
        public string? VisionBodyArabic
        {
            get => Localization.GetArabic(VisionBody);
            set => VisionBody = Localization.Serialize(VisionBodyEnglish, value);
        }
        public string? MissionTitle { get; set; }
        public string? MissionTitleEnglish
        {
            get => Localization.GetEnglish(MissionTitle);
            set => MissionTitle = Localization.Serialize(value, MissionTitleArabic);
        }
        public string? MissionTitleArabic
        {
            get => Localization.GetArabic(MissionTitle);
            set => MissionTitle = Localization.Serialize(MissionTitleEnglish, value);
        }
        public string? MissionBody { get; set; }
        public string? MissionBodyEnglish
        {
            get => Localization.GetEnglish(MissionBody);
            set => MissionBody = Localization.Serialize(value, MissionBodyArabic);
        }
        public string? MissionBodyArabic
        {
            get => Localization.GetArabic(MissionBody);
            set => MissionBody = Localization.Serialize(MissionBodyEnglish, value);
        }
        public string? ValuesTitle { get; set; }
        public string? ValuesTitleEnglish
        {
            get => Localization.GetEnglish(ValuesTitle);
            set => ValuesTitle = Localization.Serialize(value, ValuesTitleArabic);
        }
        public string? ValuesTitleArabic
        {
            get => Localization.GetArabic(ValuesTitle);
            set => ValuesTitle = Localization.Serialize(ValuesTitleEnglish, value);
        }
        public string? ValuesBody { get; set; }
        public string? ValuesBodyEnglish
        {
            get => Localization.GetEnglish(ValuesBody);
            set => ValuesBody = Localization.Serialize(value, ValuesBodyArabic);
        }
        public string? ValuesBodyArabic
        {
            get => Localization.GetArabic(ValuesBody);
            set => ValuesBody = Localization.Serialize(ValuesBodyEnglish, value);
        }
        public string? TestimonialTagline { get; set; }
        public string? TestimonialTaglineEnglish
        {
            get => Localization.GetEnglish(TestimonialTagline);
            set => TestimonialTagline = Localization.Serialize(value, TestimonialTaglineArabic);
        }
        public string? TestimonialTaglineArabic
        {
            get => Localization.GetArabic(TestimonialTagline);
            set => TestimonialTagline = Localization.Serialize(TestimonialTaglineEnglish, value);
        }
        public string? TestimonialTitle { get; set; }
        public string? TestimonialTitleEnglish
        {
            get => Localization.GetEnglish(TestimonialTitle);
            set => TestimonialTitle = Localization.Serialize(value, TestimonialTitleArabic);
        }
        public string? TestimonialTitleArabic
        {
            get => Localization.GetArabic(TestimonialTitle);
            set => TestimonialTitle = Localization.Serialize(TestimonialTitleEnglish, value);
        }
		public string? TestimonialImagePath { get; set; }
		public string? TestimonialImageAlt { get; set; }
        public IFormFile? TestimonialImage { get; set; }
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
        public string? HomeImagePath { get; set; }
        public string? HomeImageAlt { get; set; }
        public IFormFile? HomeImage { get; set; }
        public string? Count1 { get; set; }
        public string? Count1Symbol { get; set; }
        public string? Label1 { get; set; }
        public string? Label1English
        {
            get => Localization.GetEnglish(Label1);
            set => Label1 = Localization.Serialize(value, Label1Arabic);
        }
        public string? Label1Arabic
        {
            get => Localization.GetArabic(Label1);
            set => Label1 = Localization.Serialize(Label1English, value);
        }
        public string? Count1ImagePath { get; set; }
        public string? Count1ImageAlt { get; set; }
        public IFormFile? Count1Image { get; set; }
        public string? Count2 { get; set; }
        public string? Count2Symbol { get; set; }
        public string? Label2 { get; set; }
        public string? Label2English
        {
            get => Localization.GetEnglish(Label2);
            set => Label2 = Localization.Serialize(value, Label2Arabic);
        }
        public string? Label2Arabic
        {
            get => Localization.GetArabic(Label2);
            set => Label2 = Localization.Serialize(Label2English, value);
        }
        public string? Count2ImagePath { get; set; }
        public string? Count2ImageAlt { get; set; }
        public IFormFile? Count2Image { get; set; }
        public string? Count3 { get; set; }
        public string? Count3Symbol { get; set; }
        public string? Label3 { get; set; }
        public string? Label3English
        {
            get => Localization.GetEnglish(Label3);
            set => Label3 = Localization.Serialize(value, Label3Arabic);
        }
        public string? Label3Arabic
        {
            get => Localization.GetArabic(Label3);
            set => Label3 = Localization.Serialize(Label3English, value);
        }
        public string? Count3ImagePath { get; set; }
        public string? Count3ImageAlt { get; set; }
        public IFormFile? Count3Image { get; set; }
    }
}
