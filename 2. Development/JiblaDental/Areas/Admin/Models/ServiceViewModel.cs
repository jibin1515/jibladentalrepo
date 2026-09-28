using Application.Models;

namespace JiblaDental.Areas.Admin.Models
{
    public class ServiceViewModel
    {
        public ServiceSectionDto? Section { get; set; }
        public List<ServiceDto>? Services { get; set; }
    }
}
