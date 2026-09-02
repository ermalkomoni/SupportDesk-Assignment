using SupportDesk.Core.Enums;

namespace SupportDesk.Core.Entities;

public class Agent : BaseEntity
{
	public string FullName { get; set; } = default!;
	public string Email { get; set; } = default!;
	public Department Department { get; set; }
	public bool IsActive { get; set; }
	public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
