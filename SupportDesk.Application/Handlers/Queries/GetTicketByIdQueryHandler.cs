using AutoMapper;
using MediatR;
using SupportDesk.Application.DTOs.Tickets;
using SupportDesk.Application.Queries.Tickets;
using SupportDesk.Core.Repositories;

namespace SupportDesk.Application.Handlers.Queries;

public class GetTicketByIdQueryHandler : IRequestHandler<GetTicketByIdQuery, TicketDetailDto?>
{
	private readonly ITicketRepository _ticketRepository;
	private readonly IMapper _mapper;

	public GetTicketByIdQueryHandler(ITicketRepository ticketRepository, IMapper mapper)
	{
		_ticketRepository = ticketRepository;
		_mapper = mapper;
	}

	public async Task<TicketDetailDto?> Handle(GetTicketByIdQuery request, CancellationToken cancellationToken)
	{
		var ticket = await _ticketRepository.GetTicketById(request.Id, cancellationToken);
		return ticket is not null ? _mapper.Map<TicketDetailDto>(ticket) : null;
	}
}
