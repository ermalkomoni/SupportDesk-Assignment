using SupportDesk.Core.Enums;

namespace SupportDesk.Application.DTOs.Tickets;

public record TicketListItemDto(
	Guid Id,
	string Reference,
	string Title,
	string CustomerName,
	TicketPriority Priority,
	TicketStatus Status,
	Guid? AssignedAgentId,
	string? AssignedAgentName,
	DateTime CreatedDate,
	DateTime DueDate,
	bool IsOverdue
);
