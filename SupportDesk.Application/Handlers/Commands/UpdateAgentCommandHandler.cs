using AutoMapper;
using MediatR;
using SupportDesk.Application.Commands.Agents;
using SupportDesk.Application.DTOs.Agents;
using SupportDesk.Application.Exceptions;
using SupportDesk.Core.Entities;
using SupportDesk.Core.Errors;
using SupportDesk.Core.Exceptions;
using SupportDesk.Core.Repositories;

namespace SupportDesk.Application.Handlers.Commands;

public class UpdateAgentCommandHandler : IRequestHandler<UpdateAgentCommand, AgentDto>
{
	private readonly IAgentRepository _agentRepository;
	private readonly IMapper _mapper;

	public UpdateAgentCommandHandler(IAgentRepository agentRepository, IMapper mapper)
	{
		_agentRepository = agentRepository;
		_mapper = mapper;
	}

	public async Task<AgentDto> Handle(UpdateAgentCommand request, CancellationToken cancellationToken)
	{
		var dto = request.Agent;

		var agent = await _agentRepository.GetTrackedAgentById(request.Id, cancellationToken)
			?? throw new NotFoundException(nameof(Agent), request.Id);

		if (await _agentRepository.EmailExists(dto.Email, excludeId: request.Id, cancellationToken))
			throw new BusinessRuleException(AgentErrors.EmailInUse(dto.Email));

		agent.FullName = dto.FullName.Trim();
		agent.Email = dto.Email.Trim().ToLowerInvariant();
		agent.Department = dto.Department;
		agent.IsActive = dto.IsActive;

		await _agentRepository.UpdateAgent(agent, cancellationToken);

		var updated = await _agentRepository.GetAgentById(request.Id, cancellationToken);
		return _mapper.Map<AgentDto>(updated!);
	}
}

