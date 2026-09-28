using Domain.Common;

namespace Domain
{
    public class Department : OrderableBaseEntity
    {
        public string? Title { get; set; }
		public string? PageName { get; set; }
		public string? SeoTitle { get; set; }
		public string? SeoDescription { get; set; }
	}
}
