using MediatR;

namespace SupportDesk.Application.Commands.Tickets;

public record DeleteTicketCommand(Guid Id) : IRequest<bool>;

