using FluentValidation;
using SupportDesk.Application.Commands.Tickets;

namespace SupportDesk.Application.Validators.Tickets;

public class UpdateTicketCommandValidator : AbstractValidator<UpdateTicketCommand>
{
	public UpdateTicketCommandValidator()
	{
		RuleFor(x => x.Id).NotEmpty();
		RuleFor(x => x.Ticket).NotNull();

		When(x => x.Ticket is not null, () =>
		{
			RuleFor(x => x.Ticket.Title).NotEmpty().MaximumLength(200);
			RuleFor(x => x.Ticket.Description).NotEmpty().MaximumLength(4000);
			RuleFor(x => x.Ticket.CustomerName).NotEmpty().MaximumLength(200);
			RuleFor(x => x.Ticket.CustomerEmail).NotEmpty().EmailAddress().MaximumLength(256);
			RuleFor(x => x.Ticket.Priority).IsInEnum();
		});
	}
}
