using FluentValidation;
using SupportDesk.Application.Commands.Agents;

namespace SupportDesk.Application.Validators.Agents;

public class CreateAgentCommandValidator : AbstractValidator<CreateAgentCommand>
{
	public CreateAgentCommandValidator()
	{
		RuleFor(x => x.Agent).NotNull();

		When(x => x.Agent is not null, () =>
		{
			RuleFor(x => x.Agent.FullName).NotEmpty().MaximumLength(200);
			RuleFor(x => x.Agent.Email).NotEmpty().EmailAddress().MaximumLength(256);
			RuleFor(x => x.Agent.Department).IsInEnum();
		});
	}
}
