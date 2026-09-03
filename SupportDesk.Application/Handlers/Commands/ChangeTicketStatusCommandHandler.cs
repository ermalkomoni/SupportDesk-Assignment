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

public class ChangeTicketStatusCommandHandler : IRequestHandler<ChangeTicketStatusCommand, TicketDetailDto>
{
	private static readonly Dictionary<TicketStatus, TicketStatus[]> AllowedTransitions = new()
	{
		[TicketStatus.New] = new[] { TicketStatus.InProgress },
		[TicketStatus.InProgress] = new[] { TicketStatus.Resolved },
		[TicketStatus.Resolved] = new[] { TicketStatus.Closed, TicketStatus.InProgress },
		[TicketStatus.Closed] = Array.Empty<TicketStatus>(),
	};

	private readonly ITicketRepository _ticketRepository;
	private readonly IMapper _mapper;

	public ChangeTicketStatusCommandHandler(ITicketRepository ticketRepository, IMapper mapper)
	{
		_ticketRepository = ticketRepository;
		_mapper = mapper;
	}

	public async Task<TicketDetailDto> Handle(ChangeTicketStatusCommand request, CancellationToken cancellationToken)
	{
		var ticket = await _ticketRepository.GetTrackedTicketById(request.Id, cancellationToken)
			?? throw new NotFoundException(nameof(Ticket), request.Id);

		var newStatus = request.Status.NewStatus;

		if (!AllowedTransitions.TryGetValue(ticket.Status, out var allowed) || !allowed.Contains(newStatus))
			throw new InvalidStatusTransitionException(ticket.Status, newStatus);

		if (newStatus == TicketStatus.InProgress && ticket.AssignedAgent is not { IsActive: true })
			throw new BusinessRuleException(TicketErrors.InProgressRequiresActiveAgent);

		var wasResolved = ticket.Status == TicketStatus.Resolved;
		var now = DateTime.UtcNow;

		ticket.Status = newStatus;
		ticket.LastModifiedDate = now;

		ticket.ResolvedDate = newStatus switch
		{
			TicketStatus.Resolved => now,
			TicketStatus.InProgress when wasResolved => null, // reopened
			_ => ticket.ResolvedDate,
		};

		if (newStatus == TicketStatus.Closed)
			ticket.ClosedDate = now;

		await _ticketRepository.UpdateTicket(ticket, cancellationToken);

		var updated = await _ticketRepository.GetTicketById(request.Id, cancellationToken);
		return _mapper.Map<TicketDetailDto>(updated!);
	}
}
