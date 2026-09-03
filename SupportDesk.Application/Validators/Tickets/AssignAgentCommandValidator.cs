using FluentValidation;
using SupportDesk.Application.Commands.Tickets;

namespace SupportDesk.Application.Validators.Tickets;

public class AssignAgentCommandValidator : AbstractValidator<AssignAgentCommand>
{
	public AssignAgentCommandValidator()
	{
		RuleFor(x => x.TicketId).NotEmpty();
		RuleFor(x => x.AgentId).NotEmpty();
	}
}
