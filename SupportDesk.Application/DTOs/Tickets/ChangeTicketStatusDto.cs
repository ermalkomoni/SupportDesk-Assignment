using SupportDesk.Core.Enums;

namespace SupportDesk.Application.DTOs.Tickets;

public record ChangeTicketStatusDto(TicketStatus NewStatus);

