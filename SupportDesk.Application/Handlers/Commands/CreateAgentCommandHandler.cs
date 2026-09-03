using AutoMapper;
using MediatR;
using SupportDesk.Application.Commands.Agents;
using SupportDesk.Application.DTOs.Agents;
using SupportDesk.Core.Entities;
using SupportDesk.Core.Errors;
using SupportDesk.Core.Exceptions;
using SupportDesk.Core.Repositories;

namespace SupportDesk.Application.Handlers.Commands;

public class CreateAgentCommandHandler : IRequestHandler<CreateAgentCommand, AgentDto>
{
	private readonly IAgentRepository _agentRepository;
	private readonly IMapper _mapper;

	public CreateAgentCommandHandler(IAgentRepository agentRepository, IMapper mapper)
	{
		_agentRepository = agentRepository;
		_mapper = mapper;
	}

	public async Task<AgentDto> Handle(CreateAgentCommand request, CancellationToken cancellationToken)
	{
		var dto = request.Agent;

		if (await _agentRepository.EmailExists(dto.Email, excludeId: null, cancellationToken))
			throw new BusinessRuleException(AgentErrors.EmailInUse(dto.Email));

		var agent = new Agent
		{
			FullName = dto.FullName.Trim(),
			Email = dto.Email.Trim().ToLowerInvariant(),
			Department = dto.Department,
			IsActive = true,
		};

		await _agentRepository.AddAgent(agent, cancellationToken);

		var created = await _agentRepository.GetAgentById(agent.Id, cancellationToken);
		return _mapper.Map<AgentDto>(created!);
	}
}
