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
}
