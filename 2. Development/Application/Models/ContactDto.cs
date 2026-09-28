using Application.Helpers;
using Application.Models.Common;
using Microsoft.AspNetCore.Http;

namespace Application.Models
{
    public class ContactDto : BaseDto
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
        public string? Address { get; set; }
        public string? AddressEnglish
        {
            get => Localization.GetEnglish(Address);
            set => Address = Localization.Serialize(value, AddressArabic);
        }
        public string? AddressArabic
        {
            get => Localization.GetArabic(Address);
            set => Address = Localization.Serialize(AddressEnglish, value);
        }
        public string? Email1 { get; set; }
        public string? Email2 { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? FormTitle { get; set; }
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
        public string? GoogleMapLink { get; set; }
        public string? FooterTimingLabel1 { get; set; }
        public string? FooterTimingLabel1English
        {
            get => Localization.GetEnglish(FooterTimingLabel1);
            set => FooterTimingLabel1 = Localization.Serialize(value, FooterTimingLabel1Arabic);
        }
        public string? FooterTimingLabel1Arabic
        {
            get => Localization.GetArabic(FooterTimingLabel1);
            set => FooterTimingLabel1 = Localization.Serialize(FooterTimingLabel1English, value);
        }
        public string? FooterTimingLabel2 { get; set; }
        public string? FooterTimingLabel2English
        {
            get => Localization.GetEnglish(FooterTimingLabel2);
            set => FooterTimingLabel2 = Localization.Serialize(value, FooterTimingLabel2Arabic);
        }
        public string? FooterTimingLabel2Arabic
        {
            get => Localization.GetArabic(FooterTimingLabel2);
            set => FooterTimingLabel2 = Localization.Serialize(FooterTimingLabel2English, value);
        }
        public string? FooterTimingLabel3 { get; set; }
        public string? FooterTimingLabel3English
        {
            get => Localization.GetEnglish(FooterTimingLabel3);
            set => FooterTimingLabel3 = Localization.Serialize(value, FooterTimingLabel3Arabic);
        }
        public string? FooterTimingLabel3Arabic
        {
            get => Localization.GetArabic(FooterTimingLabel3);
            set => FooterTimingLabel3 = Localization.Serialize(FooterTimingLabel3English, value);
        }
        public string? FooterTimingLabel4 { get; set; }
        public string? FooterTimingLabel4English
        {
            get => Localization.GetEnglish(FooterTimingLabel4);
            set => FooterTimingLabel4 = Localization.Serialize(value, FooterTimingLabel4Arabic);
        }
        public string? FooterTimingLabel4Arabic
        {
            get => Localization.GetArabic(FooterTimingLabel4);
            set => FooterTimingLabel4 = Localization.Serialize(FooterTimingLabel4English, value);
        }
        public string? HeaderEmail { get; set; }
        public string? HeaderPhone { get; set; }
        public string? AppointmentLabel { get; set; }
        public string? AppointmentLabelEnglish
        {
            get => Localization.GetEnglish(AppointmentLabel);
            set => AppointmentLabel = Localization.Serialize(value, AppointmentLabelArabic);
        }
        public string? AppointmentLabelArabic
        {
            get => Localization.GetArabic(AppointmentLabel);
            set => AppointmentLabel = Localization.Serialize(AppointmentLabelEnglish, value);
        }
        public string? AppointmentImagePath { get; set; }
        public string? AppointmentImageAlt { get; set; }
        public IFormFile? AppointmentImage { get; set; }
        public string? AppointmentPhone { get; set; }
        public string? Certificate1ImagePath { get; set; }
        public string? Certificate1ImageAlt { get; set; }
        public IFormFile? Certificate1Image { get; set; }
        public string? Certificate2ImagePath { get; set; }
        public string? Certificate2ImageAlt { get; set; }
        public IFormFile? Certificate2Image { get; set; }
		public bool IsArabicEnabled { get; set; }
        public string? Certificate3ImagePath { get; set; }
        public string? Certificate3ImageAlt { get; set; }
        public IFormFile? Certificate3Image { get; set; }

		public string? FooterHeading1 { get; set; }
		public string? FooterHeading1English
		{
			get => Localization.GetEnglish(FooterHeading1);
			set => FooterHeading1 = Localization.Serialize(value, FooterHeading1Arabic);
		}
		public string? FooterHeading1Arabic
		{
			get => Localization.GetArabic(FooterHeading1);
			set => FooterHeading1 = Localization.Serialize(FooterHeading1English, value);
		}

		public string? FooterHeading2 { get; set; }
		public string? FooterHeading2English
		{
			get => Localization.GetEnglish(FooterHeading2);
			set => FooterHeading2 = Localization.Serialize(value, FooterHeading2Arabic);
		}
		public string? FooterHeading2Arabic
		{
			get => Localization.GetArabic(FooterHeading2);
			set => FooterHeading2 = Localization.Serialize(FooterHeading2English, value);
		}

		public string? FooterHeading3 { get; set; }
		public string? FooterHeading3English
		{
			get => Localization.GetEnglish(FooterHeading3);
			set => FooterHeading3 = Localization.Serialize(value, FooterHeading3Arabic);
		}
		public string? FooterHeading3Arabic
		{
			get => Localization.GetArabic(FooterHeading3);
			set => FooterHeading3 = Localization.Serialize(FooterHeading3English, value);
		}
	}
}
