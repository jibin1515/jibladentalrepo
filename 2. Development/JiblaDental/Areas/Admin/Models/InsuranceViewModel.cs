using Application.Models;

namespace JiblaDental.Areas.Admin.Models
{
	public class InsuranceViewModel
    {
		public InsuranceSectionDto? Section { get; set; }
		public List<InsuranceDto>? Insurances { get; set; }
	}
}
