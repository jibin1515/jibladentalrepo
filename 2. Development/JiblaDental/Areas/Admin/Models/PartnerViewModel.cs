using Application.Models;

namespace JiblaDental.Areas.Admin.Models
{
    public class PartnerViewModel
    {
        public PartnerSectionDto? Section { get; set; }
        public List<PartnerCategoryDto>? PartnerCategorys { get; set; }
        public PartnerCategoryDto? PartnerCategory { get; set; }
        public List<PartnerDto>? Partner { get; set; }
    }
}
