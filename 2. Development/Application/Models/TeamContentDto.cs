using Application.Helpers;
using Application.Models.Common;
using Microsoft.AspNetCore.Http;

namespace Application.Models
{
    public class TeamContentDto : OrderableDto
    {
        public long TeamId { get; set; }
        public string? Type { get; set; }//social,service,qualification,gallery
        public string? Name { get; set; }
        public string? Url { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public string? ImagePath { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageAlt { get; set; }
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
