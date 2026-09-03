using MediatR;
using SupportDesk.Application.DTOs.Agents;

namespace SupportDesk.Application.Queries.Agents;

public record GetAllAgentsQuery(string? Search) : IRequest<IEnumerable<AgentDto>>;
