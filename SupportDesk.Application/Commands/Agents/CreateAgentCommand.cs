using MediatR;
using SupportDesk.Application.DTOs.Agents;

namespace SupportDesk.Application.Commands.Agents;

public record CreateAgentCommand(CreateAgentDto Agent) : IRequest<AgentDto>;
