using SupportDesk.Application.DTOs.Comments;
using SupportDesk.Core.Enums;

namespace SupportDesk.Application.DTOs.Tickets;

public record TicketDetailDto(
	Guid Id,
	string Reference,
	string Title,
	string Description,
	string CustomerName,
	string CustomerEmail,
	TicketPriority Priority,
	TicketStatus Status,
	Guid? AssignedAgentId,
	string? AssignedAgentName,
	DateTime CreatedDate,
	DateTime LastModifiedDate,
	DateTime? ResolvedDate,
	DateTime? ClosedDate,
	DateTime DueDate,
	bool IsOverdue,
	IReadOnlyList<TicketStatus> AllowedTransitions,
	IReadOnlyCollection<CommentDto> Comments
);
