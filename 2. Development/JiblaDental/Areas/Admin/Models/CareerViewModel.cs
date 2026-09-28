using Application.Models;

namespace JiblaDental.Areas.Admin.Models
{
    public class CareerViewModel
    {
        public CareerSectionDto? Section { get; set; }
        public List<CareerDto>? Careers { get; set; }
        public CareerDto? Career { get; set; }
        public List<CareerContentDto>? CareersContent { get; set; }
    }
}
