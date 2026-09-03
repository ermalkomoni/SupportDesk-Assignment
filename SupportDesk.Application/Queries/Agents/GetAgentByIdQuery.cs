using MediatR;
using SupportDesk.Application.DTOs.Agents;

namespace SupportDesk.Application.Queries.Agents;

public record GetAgentByIdQuery(Guid Id) : IRequest<AgentDto?>;

