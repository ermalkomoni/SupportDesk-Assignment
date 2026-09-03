using MediatR;
using SupportDesk.Application.Commands.Tickets;
using SupportDesk.Core.Enums;
using SupportDesk.Core.Errors;
using SupportDesk.Core.Exceptions;
using SupportDesk.Core.Repositories;

namespace SupportDesk.Application.Handlers.Commands;

public class DeleteTicketCommandHandler : IRequestHandler<DeleteTicketCommand, bool>
{
	private readonly ITicketRepository _ticketRepository;

	public DeleteTicketCommandHandler(ITicketRepository ticketRepository)
	{
		_ticketRepository = ticketRepository;
	}

	public async Task<bool> Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
	{
		var ticket = await _ticketRepository.GetTrackedTicketById(request.Id, cancellationToken);
		if (ticket is null)
			return false;

		if (ticket.Status == TicketStatus.Closed)
			throw new BusinessRuleException(TicketErrors.Closed);

		await _ticketRepository.DeleteTicket(ticket, cancellationToken);
		return true;
	}
}
