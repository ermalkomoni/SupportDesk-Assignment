using MediatR;
using SupportDesk.Application.DTOs.Tickets;

namespace SupportDesk.Application.Commands.Tickets;

public record AssignAgentCommand(Guid TicketId, Guid AgentId) : IRequest<TicketDetailDto>;

