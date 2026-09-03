using MediatR;
using SupportDesk.Application.DTOs.Tickets;
using SupportDesk.Core.Specs;

namespace SupportDesk.Application.Queries.Tickets;

public record GetPaginatedTicketsQuery(TicketSpecParams TicketSpecParams) : IRequest<Pagination<TicketListItemDto>>;