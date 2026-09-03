using MediatR;
using SupportDesk.Application.DTOs.Tickets;

namespace SupportDesk.Application.Commands.Tickets;

public record UnassignAgentCommand(Guid TicketId) : IRequest<TicketDetailDto>;

