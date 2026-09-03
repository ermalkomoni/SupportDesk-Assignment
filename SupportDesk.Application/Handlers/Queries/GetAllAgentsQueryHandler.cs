using AutoMapper;
using MediatR;
using SupportDesk.Application.DTOs.Agents;
using SupportDesk.Application.Queries.Agents;
using SupportDesk.Core.Repositories;

namespace SupportDesk.Application.Handlers.Queries;

public class GetAllAgentsQueryHandler : IRequestHandler<GetAllAgentsQuery, IEnumerable<AgentDto>>
{
	private readonly IAgentRepository _agentRepository;
	private readonly IMapper _mapper;

	public GetAllAgentsQueryHandler(IAgentRepository agentRepository, IMapper mapper)
	{
		_agentRepository = agentRepository;
		_mapper = mapper;
	}

	public async Task<IEnumerable<AgentDto>> Handle(GetAllAgentsQuery request, CancellationToken cancellationToken)
	{
		var agents = await _agentRepository.GetAllAgents(request.Search, cancellationToken);
		return _mapper.Map<IEnumerable<AgentDto>>(agents);
	}
}
