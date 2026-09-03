using AutoMapper;
using MediatR;
using SupportDesk.Application.DTOs.Tickets;
using SupportDesk.Application.Queries.Tickets;
using SupportDesk.Core.Repositories;
using SupportDesk.Core.Specs;

namespace SupportDesk.Application.Handlers.Queries;

public class GetPaginatedTicketsQueryHandler : IRequestHandler<GetPaginatedTicketsQuery, Pagination<TicketListItemDto>>
{
	private readonly ITicketRepository _ticketRepository;
	private readonly IMapper _mapper;

	public GetPaginatedTicketsQueryHandler(ITicketRepository ticketRepository, IMapper mapper)
	{
		_ticketRepository = ticketRepository;
		_mapper = mapper;
	}

	public async Task<Pagination<TicketListItemDto>> Handle(GetPaginatedTicketsQuery request, CancellationToken cancellationToken)
	{
		var paginatedTickets = await _ticketRepository.GetPaginatedTickets(request.TicketSpecParams, cancellationToken);

		var ticketDtos = _mapper.Map<IReadOnlyList<TicketListItemDto>>(paginatedTickets.Items);

		return new Pagination<TicketListItemDto>(
			ticketDtos,
			paginatedTickets.PageNumber,
			paginatedTickets.PageSize,
			paginatedTickets.TotalCount,
			paginatedTickets.TotalPages);
	}
}
