using Application.Models;

namespace JiblaDental.Areas.Admin.Models
{
    public class LoyaltyViewModel
    {
        public LoyaltyDto? Loyalty { get; set; }
        public List<LoyaltyImageDto>? LoyaltyImages { get; set; }
    }
}
