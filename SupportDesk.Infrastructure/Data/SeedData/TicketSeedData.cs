using Microsoft.EntityFrameworkCore;
using SupportDesk.Core;
using SupportDesk.Core.Entities;
using SupportDesk.Core.Enums;

namespace SupportDesk.Infrastructure.Data.SeedData;

public static class TicketSeedData
{
	public static void Seed(ModelBuilder modelBuilder)
	{
		// Dated a week back so High/Critical (and, later, Normal) tickets that are still open sit past their SLA.
		var created = new DateTime(2026, 8, 27, 9, 0, 0, DateTimeKind.Utc);
		var tickets = new List<Ticket>();

		for (var i = 0; i < 20; i++)
		{
			var status = (TicketStatus)(i % 4);
			var priority = (TicketPriority)(i / 4 % 4);

			tickets.Add(new Ticket
			{
				Id = new Guid($"11111111-0000-0000-0000-{i + 1:D12}"),
				Reference = $"TCK-2026-{i + 1:D4}",
				Title = $"Sample ticket {i + 1}",
				Description = "Seeded ticket for demo purposes.",
				CustomerName = "PECB Customer",
				CustomerEmail = "support@pecb.com",
				Priority = priority,
				Status = status,
				CreatedDate = created,
				LastModifiedDate = created,
				DueDate = TicketDueDate.For(priority, created),
				AssignedAgentId = status == TicketStatus.New ? null : AgentSeedData.Ids[i % 4],
				ResolvedDate = status is TicketStatus.Resolved or TicketStatus.Closed ? created.AddDays(1) : null,
				ClosedDate = status == TicketStatus.Closed ? created.AddDays(2) : null,
			});
		}

		modelBuilder.Entity<Ticket>().HasData(tickets);
	}
}

