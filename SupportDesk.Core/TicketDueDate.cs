using SupportDesk.Core.Enums;

namespace SupportDesk.Core;

public static class TicketDueDate
{
	public static DateTime For(TicketPriority priority, DateTime createdDate) => priority switch
	{
		TicketPriority.Critical => createdDate.AddHours(4),
		TicketPriority.High => createdDate.AddDays(1),
		TicketPriority.Normal => createdDate.AddDays(3),
		TicketPriority.Low => createdDate.AddDays(7),
		_ => throw new ArgumentOutOfRangeException(nameof(priority), priority, "Unknown ticket priority.")
	};
}
