using FluentValidation;
using SupportDesk.Application.Commands.Tickets;

namespace SupportDesk.Application.Validators.Tickets;

public class AddCommentCommandValidator : AbstractValidator<AddCommentCommand>
{
	public AddCommentCommandValidator()
	{
		RuleFor(x => x.TicketId).NotEmpty();
		RuleFor(x => x.Comment).NotNull();

		When(x => x.Comment is not null, () =>
		{
			RuleFor(x => x.Comment.AuthorName).NotEmpty().MaximumLength(200);
			RuleFor(x => x.Comment.Body).NotEmpty().MaximumLength(4000);
		});
	}
}
