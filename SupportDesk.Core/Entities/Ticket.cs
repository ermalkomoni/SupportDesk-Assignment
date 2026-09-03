using SupportDesk.Core.Enums;

namespace SupportDesk.Core.Entities;

public class Ticket : BaseEntity
{
	public string Reference { get; set; } = default!;
	public string Title { get; set; } = default!;
	public string Description { get; set; } = default!;
	public string CustomerName { get; set; } = default!;
	public string CustomerEmail { get; set; } = default!;
	public TicketPriority Priority { get; set; }
	public TicketStatus Status { get; set; }

	public Guid? AssignedAgentId { get; set; }
	public Agent? AssignedAgent { get; set; }

	public DateTime CreatedDate { get; set; }
	public DateTime LastModifiedDate { get; set; }
	public DateTime? ResolvedDate { get; set; }
	public DateTime? ClosedDate { get; set; }
	public DateTime DueDate { get; set; }

	public bool IsOverdue =>
	Status is not (TicketStatus.Resolved or TicketStatus.Closed) && DateTime.UtcNow > DueDate;

	public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
