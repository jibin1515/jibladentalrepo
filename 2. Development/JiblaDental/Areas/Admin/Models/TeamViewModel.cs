using Application.Models;

namespace JiblaDental.Areas.Admin.Models
{
    public class TeamViewModel
    {
        public TeamSectionDto? Section { get; set; }
        public List<DepartmentDto>? Departments { get; set; }
        public DepartmentDto? Department { get; set; }
        public List<TeamDto>? Teams { get; set; }
        public TeamDto? Team { get; set; }
        public string? Type { get; set; }
        public List<TeamContentDto>? Content { get; set; }
    }
}
