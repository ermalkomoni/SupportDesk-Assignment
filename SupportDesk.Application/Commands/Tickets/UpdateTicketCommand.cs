using MediatR;
using SupportDesk.Application.DTOs.Tickets;

namespace SupportDesk.Application.Commands.Tickets;

public record UpdateTicketCommand(Guid Id, UpdateTicketDto Ticket) : IRequest<TicketDetailDto>;

