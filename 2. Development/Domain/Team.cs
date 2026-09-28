using Domain.Common;

namespace Domain
{
    public class Team : OrderableBaseEntity
    {
        public long DepartmentId { get; set; }
        public string? Name { get; set; }
        public string? Body { get; set; }
        public string? ShortBody { get; set; }
        public string? ImagePath { get; set; }
		public string? ImageAlt { get; set; }
		public bool ShowOnHomePage { get; set; }
		public string? PageName { get; set; }
		public string? SeoTitle { get; set; }
		public string? SeoDescription { get; set; }
        public string? DepartmentIds { get; set; }
    }
}
