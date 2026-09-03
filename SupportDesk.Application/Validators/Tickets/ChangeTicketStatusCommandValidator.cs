using FluentValidation;
using SupportDesk.Application.Commands.Tickets;

namespace SupportDesk.Application.Validators.Tickets;

public class ChangeTicketStatusCommandValidator : AbstractValidator<ChangeTicketStatusCommand>
{
	public ChangeTicketStatusCommandValidator()
	{
		RuleFor(x => x.Id).NotEmpty();
		RuleFor(x => x.Status).NotNull();

		When(x => x.Status is not null, () =>
		{
			RuleFor(x => x.Status.NewStatus).IsInEnum();
		});
	}
}
