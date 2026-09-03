using MediatR;
using SupportDesk.Application.DTOs.Tickets;

namespace SupportDesk.Application.Commands.Tickets;

public record CreateTicketCommand(CreateTicketDto Ticket) : IRequest<TicketDetailDto>;

