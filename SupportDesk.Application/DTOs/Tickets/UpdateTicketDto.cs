using SupportDesk.Core.Enums;

namespace SupportDesk.Application.DTOs.Tickets;

public record UpdateTicketDto(
	string Title,
	string Description,
	string CustomerName,
	string CustomerEmail,
	TicketPriority Priority);
