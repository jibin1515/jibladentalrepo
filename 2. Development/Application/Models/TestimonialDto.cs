using Application.Helpers;
using Application.Models.Common;
using Microsoft.AspNetCore.Http;

namespace Application.Models
{
    public class TestimonialDto : OrderableDto
    {
        public string? Name { get; set; }
        public string? Message { get; set; }
        public string? ImagePath { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageAlt { get; set; }
        public string? NameEnglish
        {
            get => Localization.GetEnglish(Name);
            set => Name = Localization.Serialize(value, NameArabic);
        }
        public string? NameArabic
        {
            get => Localization.GetArabic(Name);
            set => Name = Localization.Serialize(NameEnglish, value);
        }
        public string? MessageEnglish
        {
            get => Localization.GetEnglish(Message);
            set => Message = Localization.Serialize(value, MessageArabic);
        }
        public string? MessageArabic
        {
            get => Localization.GetArabic(Message);
            set => Message = Localization.Serialize(MessageEnglish, value);
        }
    }
}
