using Application.Models;

namespace JiblaDental.Areas.Admin.Models
{
    public class AboutViewModel
    {
        public AboutDto? Section { get; set; }
        public List<AboutGalleryDto>? AboutGallerys { get; set; }
        public List<TestimonialDto>? Testimonials { get; set; }
    }
}
