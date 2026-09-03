using MediatR;

namespace SupportDesk.Application.Commands.Agents;

public record DeleteAgentCommand(Guid Id) : IRequest<bool>;
