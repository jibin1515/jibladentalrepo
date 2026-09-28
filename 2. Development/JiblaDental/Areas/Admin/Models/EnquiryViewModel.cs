using Application.Models;
using Application.Models.Framework;

namespace JiblaDental.Areas.Admin.Models;

public class EnquiryViewModel
{
    public EnquiryDto? Enquiry { get; set; }
    public List<EnquiryDto>? Enquiries { get; set; }
    public EmailDto? Email { get; set; }
    public CareerDto? Career { get; set; }
    public List<CareerDto>? Careers { get; set; }
}