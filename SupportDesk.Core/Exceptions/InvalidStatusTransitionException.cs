using SupportDesk.Core.Enums;

namespace SupportDesk.Core.Exceptions;

public class InvalidStatusTransitionException : BusinessRuleException
{
	public TicketStatus From { get; }
	public TicketStatus To { get; }

	public InvalidStatusTransitionException(TicketStatus from, TicketStatus to)
		: base($"Cannot transition ticket from '{from}' to '{to}'.")
	{
		From = from;
		To = to;
	}
}
