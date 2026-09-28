using Domain.Common;

namespace Domain;

public class Enquiry : BaseEntity
{
    public string? Name { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Message { get; set; }
    public string? Purpose { get; set; }
    public string? Subject { get; set; }
    public long? EntityId { get; set; }
    public string? AttachedFilePath { get; set; }
}