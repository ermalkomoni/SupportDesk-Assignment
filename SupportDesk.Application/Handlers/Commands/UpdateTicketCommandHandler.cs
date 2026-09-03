using AutoMapper;
using MediatR;
using SupportDesk.Application.Commands.Tickets;
using SupportDesk.Application.DTOs.Tickets;
using SupportDesk.Application.Exceptions;
using SupportDesk.Core;
using SupportDesk.Core.Entities;
using SupportDesk.Core.Enums;
using SupportDesk.Core.Errors;
using SupportDesk.Core.Exceptions;
using SupportDesk.Core.Repositories;

namespace SupportDesk.Application.Handlers.Commands;

public class UpdateTicketCommandHandler : IRequestHandler<UpdateTicketCommand, TicketDetailDto>
{
	private readonly ITicketRepository _ticketRepository;
	private readonly IMapper _mapper;

	public UpdateTicketCommandHandler(ITicketRepository ticketRepository, IMapper mapper)
	{
		_ticketRepository = ticketRepository;
		_mapper = mapper;
	}

	public async Task<TicketDetailDto> Handle(UpdateTicketCommand request, CancellationToken cancellationToken)
	{
		var dto = request.Ticket;

		var ticket = await _ticketRepository.GetTrackedTicketById(request.Id, cancellationToken)
			?? throw new NotFoundException(nameof(Ticket), request.Id);

		if (ticket.Status == TicketStatus.Closed)
			throw new BusinessRuleException(TicketErrors.Closed);

		ticket.Title = dto.Title.Trim();
		ticket.Description = dto.Description.Trim();
		ticket.CustomerName = dto.CustomerName.Trim();
		ticket.CustomerEmail = dto.CustomerEmail.Trim().ToLowerInvariant();

		if (ticket.Priority != dto.Priority)
		{
			ticket.Priority = dto.Priority;
			ticket.DueDate = TicketDueDate.For(dto.Priority, ticket.CreatedDate);
		}

		ticket.LastModifiedDate = DateTime.UtcNow;

		await _ticketRepository.UpdateTicket(ticket, cancellationToken);

		var updated = await _ticketRepository.GetTicketById(request.Id, cancellationToken);
		return _mapper.Map<TicketDetailDto>(updated!);
	}
}
