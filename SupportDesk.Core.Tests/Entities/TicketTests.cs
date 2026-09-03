using SupportDesk.Core;
using SupportDesk.Core.Entities;
using SupportDesk.Core.Enums;

namespace SupportDesk.Core.Tests.Entities;

public class TicketTests
{
	[Fact]
	public void AllowedTransitions_WhenClosed_IsEmpty()
	{
		var ticket = new Ticket { Status = TicketStatus.Closed };

		Assert.Empty(ticket.AllowedTransitions);
	}

	[Fact]
	public void AllowedTransitions_WhenNewWithoutActiveAgent_IsEmpty()
	{
		var ticket = new Ticket { Status = TicketStatus.New };

		Assert.Empty(ticket.AllowedTransitions);
	}

	[Fact]
	public void AllowedTransitions_WhenNewWithActiveAgent_ContainsInProgress()
	{
		var ticket = new Ticket
		{
			Status = TicketStatus.New,
			AssignedAgent = new Agent { IsActive = true },
		};

		Assert.Equal([TicketStatus.InProgress], ticket.AllowedTransitions);
	}

	[Fact]
	public void AllowedTransitions_WhenInProgress_ContainsResolved()
	{
		var ticket = new Ticket { Status = TicketStatus.InProgress };

		Assert.Equal([TicketStatus.Resolved], ticket.AllowedTransitions);
	}

	[Fact]
	public void AllowedTransitions_WhenResolvedWithoutActiveAgent_ContainsOnlyClosed()
	{
		var ticket = new Ticket { Status = TicketStatus.Resolved };

		Assert.Equal([TicketStatus.Closed], ticket.AllowedTransitions);
	}

	[Fact]
	public void AllowedTransitions_WhenResolvedWithActiveAgent_AllowsCloseAndReopen()
	{
		var ticket = new Ticket
		{
			Status = TicketStatus.Resolved,
			AssignedAgent = new Agent { IsActive = true },
		};

		Assert.Equal(
			[TicketStatus.Closed, TicketStatus.InProgress],
			ticket.AllowedTransitions);
	}

	[Theory]
	[InlineData(TicketPriority.Critical, 4, 0)]
	[InlineData(TicketPriority.High, 0, 1)]
	[InlineData(TicketPriority.Normal, 0, 3)]
	[InlineData(TicketPriority.Low, 0, 7)]
	public void For_WhenPriorityProvided_ReturnsExpectedDueDate(
		TicketPriority priority,
		int expectedHours,
		int expectedDays)
	{
		var createdDate = new DateTime(2026, 9, 3, 9, 30, 0, DateTimeKind.Utc);
		var expectedDueDate = createdDate.AddHours(expectedHours).AddDays(expectedDays);

		var dueDate = TicketDueDate.For(priority, createdDate);

		Assert.Equal(expectedDueDate, dueDate);
	}
}
