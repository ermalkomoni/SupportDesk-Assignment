using MediatR;
using SupportDesk.Application.DTOs.Tickets;

namespace SupportDesk.Application.Commands.Tickets;

public record ChangeTicketStatusCommand(Guid Id, ChangeTicketStatusDto Status) : IRequest<TicketDetailDto>;

