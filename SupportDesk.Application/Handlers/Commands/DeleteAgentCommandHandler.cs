using MediatR;
using SupportDesk.Application.Commands.Agents;
using SupportDesk.Core.Errors;
using SupportDesk.Core.Exceptions;
using SupportDesk.Core.Repositories;

namespace SupportDesk.Application.Handlers.Commands;

public class DeleteAgentCommandHandler : IRequestHandler<DeleteAgentCommand, bool>
{
	private readonly IAgentRepository _agentRepository;

	public DeleteAgentCommandHandler(IAgentRepository agentRepository)
	{
		_agentRepository = agentRepository;
	}

	public async Task<bool> Handle(DeleteAgentCommand request, CancellationToken cancellationToken)
	{
		var agent = await _agentRepository.GetTrackedAgentById(request.Id, cancellationToken);
		if (agent is null)
			return false;

		if (await _agentRepository.HasAssignedTickets(request.Id, cancellationToken))
			throw new BusinessRuleException(AgentErrors.HasAssignedTickets);

		await _agentRepository.DeleteAgent(agent, cancellationToken);
		return true;
	}
}
