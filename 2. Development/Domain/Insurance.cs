using Domain.Common;

namespace Domain
{
	public class Insurance : OrderableBaseEntity
	{
		public string? Title { get; set; }
		public string? ImagePath { get; set; }
		public string? ImageAlt { get; set; }
		public string? Url { get; set; }
	}
}
