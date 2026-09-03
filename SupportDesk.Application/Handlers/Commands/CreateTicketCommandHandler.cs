using AutoMapper;
using MediatR;
using SupportDesk.Application.Commands.Tickets;
using SupportDesk.Application.DTOs.Tickets;
using SupportDesk.Application.Services;
using SupportDesk.Core;
using SupportDesk.Core.Entities;
using SupportDesk.Core.Enums;
using SupportDesk.Core.Repositories;

namespace SupportDesk.Application.Handlers.Commands;

public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, TicketDetailDto>
{
	private readonly ITicketRepository _ticketRepository;
	private readonly IReferenceNumberGenerator _referenceNumberGenerator;
	private readonly IMapper _mapper;

	public CreateTicketCommandHandler(
		ITicketRepository ticketRepository,
		IReferenceNumberGenerator referenceNumberGenerator,
		IMapper mapper)
	{
		_ticketRepository = ticketRepository;
		_referenceNumberGenerator = referenceNumberGenerator;
		_mapper = mapper;
	}

	public async Task<TicketDetailDto> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
	{
		var dto = request.Ticket;
		var now = DateTime.UtcNow;

		var ticket = new Ticket
		{
			Reference = await _referenceNumberGenerator.GenerateAsync(cancellationToken),
			Title = dto.Title.Trim(),
			Description = dto.Description.Trim(),
			CustomerName = dto.CustomerName.Trim(),
			CustomerEmail = dto.CustomerEmail.Trim().ToLowerInvariant(),
			Priority = dto.Priority,
			Status = TicketStatus.New,
			CreatedDate = now,
			LastModifiedDate = now,
			DueDate = TicketDueDate.For(dto.Priority, now),
		};

		await _ticketRepository.AddTicket(ticket, cancellationToken);

		var created = await _ticketRepository.GetTicketById(ticket.Id, cancellationToken);
		return _mapper.Map<TicketDetailDto>(created!);
	}
}
