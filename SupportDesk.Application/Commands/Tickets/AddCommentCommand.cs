using MediatR;
using SupportDesk.Application.DTOs.Comments;

namespace SupportDesk.Application.Commands.Tickets;

public record AddCommentCommand(Guid TicketId, CreateCommentDto Comment) : IRequest<CommentDto>;

