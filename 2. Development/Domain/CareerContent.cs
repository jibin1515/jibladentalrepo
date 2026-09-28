using Domain.Common;

namespace Domain
{
	public class CareerContent : OrderableBaseEntity
	{
		public long CareerId { get; set; }
		public string? Title { get; set; }
		public string? Body { get; set; }
	}
}
