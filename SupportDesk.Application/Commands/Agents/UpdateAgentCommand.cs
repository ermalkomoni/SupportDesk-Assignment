using MediatR;
using SupportDesk.Application.DTOs.Agents;

namespace SupportDesk.Application.Commands.Agents;

public record UpdateAgentCommand(Guid Id, UpdateAgentDto Agent) : IRequest<AgentDto>;