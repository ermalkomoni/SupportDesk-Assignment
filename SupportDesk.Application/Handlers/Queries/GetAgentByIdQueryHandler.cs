using AutoMapper;
using MediatR;
using SupportDesk.Application.DTOs.Agents;
using SupportDesk.Application.Queries.Agents;
using SupportDesk.Core.Repositories;

namespace SupportDesk.Application.Handlers.Queries;

public class GetAgentByIdQueryHandler : IRequestHandler<GetAgentByIdQuery, AgentDto?>
{
	private readonly IAgentRepository _agentRepository;
	private readonly IMapper _mapper;

	public GetAgentByIdQueryHandler(IAgentRepository agentRepository, IMapper mapper)
	{
		_agentRepository = agentRepository;
		_mapper = mapper;
	}

	public async Task<AgentDto?> Handle(GetAgentByIdQuery request, CancellationToken cancellationToken)
	{
		var agent = await _agentRepository.GetAgentById(request.Id, cancellationToken);
		return agent is not null ? _mapper.Map<AgentDto>(agent) : null;
	}
}