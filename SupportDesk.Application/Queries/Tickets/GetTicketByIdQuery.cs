using MediatR;
using SupportDesk.Application.DTOs.Tickets;

namespace SupportDesk.Application.Queries.Tickets;

public record GetTicketByIdQuery(Guid Id) : IRequest<TicketDetailDto?>;

