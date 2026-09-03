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

public class UnassignAgentCommandHandler : IRequestHandler<UnassignAgentCommand, TicketDetailDto>
{
	private readonly ITicketRepository _ticketRepository;
	private readonly IMapper _mapper;

	public UnassignAgentCommandHandler(ITicketRepository ticketRepository, IMapper mapper)
	{
		_ticketRepository = ticketRepository;
		_mapper = mapper;
	}

	public async Task<TicketDetailDto> Handle(UnassignAgentCommand request, CancellationToken cancellationToken)
	{
		var ticket = await _ticketRepository.GetTrackedTicketById(request.TicketId, cancellationToken)
			?? throw new NotFoundException(nameof(Ticket), request.TicketId);

		if (ticket.Status == TicketStatus.Closed)
			throw new BusinessRuleException(TicketErrors.Closed);

		if (ticket.Status == TicketStatus.InProgress)
			throw new BusinessRuleException(TicketErrors.CannotUnassignInProgress);

		ticket.AssignedAgentId = null;
		ticket.AssignedAgent = null;
		ticket.LastModifiedDate = DateTime.UtcNow;

		await _ticketRepository.UpdateTicket(ticket, cancellationToken);

		var updated = await _ticketRepository.GetTicketById(request.TicketId, cancellationToken);
		return _mapper.Map<TicketDetailDto>(updated!);
	}
}
