using AutoMapper;
using MediatR;
using SupportDesk.Application.Commands.Tickets;
using SupportDesk.Application.DTOs.Tickets;
using SupportDesk.Application.Exceptions;
using SupportDesk.Core.Entities;
using SupportDesk.Core.Enums;
using SupportDesk.Core.Errors;
using SupportDesk.Core.Exceptions;
using SupportDesk.Core.Repositories;

namespace SupportDesk.Application.Handlers.Commands;

public class AssignAgentCommandHandler : IRequestHandler<AssignAgentCommand, TicketDetailDto>
{
	private readonly ITicketRepository _ticketRepository;
	private readonly IAgentRepository _agentRepository;
	private readonly IMapper _mapper;

	public AssignAgentCommandHandler(
		ITicketRepository ticketRepository,
		IAgentRepository agentRepository,
		IMapper mapper)
	{
		_ticketRepository = ticketRepository;
		_agentRepository = agentRepository;
		_mapper = mapper;
	}

	public async Task<TicketDetailDto> Handle(AssignAgentCommand request, CancellationToken cancellationToken)
	{
		var ticket = await _ticketRepository.GetTrackedTicketById(request.TicketId, cancellationToken)
			?? throw new NotFoundException(nameof(Ticket), request.TicketId);

		var agent = await _agentRepository.GetTrackedAgentById(request.AgentId, cancellationToken)
			?? throw new NotFoundException(nameof(Agent), request.AgentId);

		if (ticket.Status == TicketStatus.Closed)
			throw new BusinessRuleException(TicketErrors.Closed);

		if (!agent.IsActive)
			throw new BusinessRuleException(TicketErrors.InactiveAgent(agent.FullName));

		ticket.AssignedAgentId = agent.Id;
		ticket.AssignedAgent = agent;
		ticket.LastModifiedDate = DateTime.UtcNow;

		await _ticketRepository.UpdateTicket(ticket, cancellationToken);

		var updated = await _ticketRepository.GetTicketById(request.TicketId, cancellationToken);
		return _mapper.Map<TicketDetailDto>(updated!);
	}
}
