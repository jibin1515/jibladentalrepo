using Application.Models;

namespace JiblaDental.Models
{
    public class FooterViewModel
    {
        public List<PageSettingsDto>? Pages { get; set; }
        public List<SocialMediaDto>? SocialMedia { get; set; }
        public ContactDto? Contact { get; set; }
        public AboutDto? About { get; set; }
	}
}
