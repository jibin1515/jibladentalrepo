using Domain.Common;

namespace Domain
{
    public class Contact : BaseEntity
    {
        public string? Title { get; set; }
        public string? Address { get; set; }
        public string? Email1 { get; set; }
        public string? Email2 { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? FormTitle { get; set; }
        public string? GoogleMapLink { get; set; }
        public string? FooterTimingLabel1 { get; set; }
        public string? FooterTimingLabel2 { get; set; }
        public string? FooterTimingLabel3 { get; set; }
        public string? FooterTimingLabel4 { get; set; }
        public string? HeaderEmail { get; set; }
        public string? HeaderPhone { get; set; }
        public string? AppointmentLabel { get; set; }
        public string? AppointmentImagePath { get; set; }
        public string? AppointmentImageAlt { get; set; }
        public string? AppointmentPhone { get; set; }
        public string? Certificate1ImagePath { get; set; }
        public string? Certificate1ImageAlt { get; set; }
        public string? Certificate2ImagePath { get; set; }
        public string? Certificate2ImageAlt { get; set; }
        public bool IsArabicEnabled { get; set; }
        public string? Certificate3ImagePath { get; set; }
        public string? Certificate3ImageAlt { get; set; }
        public string? FooterHeading1 { get; set; }
        public string? FooterHeading2 { get; set ; }
        public string? FooterHeading3 { get; set ; }
	}
}
