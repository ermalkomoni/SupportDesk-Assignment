using AutoMapper;
using MediatR;
using SupportDesk.Application.Commands.Tickets;
using SupportDesk.Application.DTOs.Comments;
using SupportDesk.Application.Exceptions;
using SupportDesk.Core.Entities;
using SupportDesk.Core.Enums;
using SupportDesk.Core.Errors;
using SupportDesk.Core.Exceptions;
using SupportDesk.Core.Repositories;

namespace SupportDesk.Application.Handlers.Commands;

public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, CommentDto>
{
	private readonly ITicketRepository _ticketRepository;
	private readonly IMapper _mapper;

	public AddCommentCommandHandler(ITicketRepository ticketRepository, IMapper mapper)
	{
		_ticketRepository = ticketRepository;
		_mapper = mapper;
	}

	public async Task<CommentDto> Handle(AddCommentCommand request, CancellationToken cancellationToken)
	{
		var ticket = await _ticketRepository.GetTrackedTicketById(request.TicketId, cancellationToken)
			?? throw new NotFoundException(nameof(Ticket), request.TicketId);

		if (ticket.Status == TicketStatus.Closed)
			throw new BusinessRuleException(TicketErrors.Closed);

		var comment = new Comment
		{
			TicketId = ticket.Id,
			AuthorName = request.Comment.AuthorName.Trim(),
			Body = request.Comment.Body.Trim(),
			CreatedDate = DateTime.UtcNow,
		};

		ticket.Comments.Add(comment);
		ticket.LastModifiedDate = DateTime.UtcNow;

		await _ticketRepository.UpdateTicket(ticket, cancellationToken);

		return _mapper.Map<CommentDto>(comment);
	}
}
