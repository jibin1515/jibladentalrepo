using Application.Models;

namespace JiblaDental.Models
{
    public class HeaderViewModel
    {
        public List<PageSettingsDto>? Pages { get; set; }
        public List<SocialMediaDto>? SocialMedia { get; set; }
        public ContactDto? Contact { get; set; }
    }
}
