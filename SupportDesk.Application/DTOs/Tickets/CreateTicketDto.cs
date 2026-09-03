using SupportDesk.Core.Enums;

namespace SupportDesk.Application.DTOs.Tickets;

public record CreateTicketDto(
	string Title,
	string Description,
	string CustomerName,
	string CustomerEmail,
	TicketPriority Priority);
