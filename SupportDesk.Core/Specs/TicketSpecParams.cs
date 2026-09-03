using SupportDesk.Core.Enums;

namespace SupportDesk.Core.Specs;

public class TicketSpecParams : PaginationParams
{
	public TicketStatus? Status { get; set; }
	public TicketPriority? Priority { get; set; }
	public Guid? AssignedAgentId { get; set; }
	public bool OverdueOnly { get; set; }
}

