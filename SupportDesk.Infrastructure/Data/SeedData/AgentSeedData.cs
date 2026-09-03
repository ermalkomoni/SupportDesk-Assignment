using Microsoft.EntityFrameworkCore;
using SupportDesk.Core.Entities;
using SupportDesk.Core.Enums;

namespace SupportDesk.Infrastructure.Data.SeedData;

public static class AgentSeedData
{
	public static readonly Guid[] Ids =
	{
		new("a0000000-0000-0000-0000-000000000001"),
		new("a0000000-0000-0000-0000-000000000002"),
		new("a0000000-0000-0000-0000-000000000003"),
		new("a0000000-0000-0000-0000-000000000004"),
		new("a0000000-0000-0000-0000-000000000005"),
	};

	public static void Seed(ModelBuilder modelBuilder) =>
		modelBuilder.Entity<Agent>().HasData(
			new Agent { Id = Ids[0], FullName = "Ermal Komoni", Email = "ermal@pecb.com", Department = Department.Technical, IsActive = true },
			new Agent { Id = Ids[1], FullName = "Njomza PECB", Email = "njomza@pecb.com", Department = Department.Billing, IsActive = true },
			new Agent { Id = Ids[2], FullName = "PECB Test User 1", Email = "pecb-test-user1@pecb.com", Department = Department.Technical, IsActive = true },
			new Agent { Id = Ids[3], FullName = "PECB Test User 2", Email = "pecb-test-user2@pecb.com", Department = Department.General, IsActive = true },
			new Agent { Id = Ids[4], FullName = "PECB Test User 3", Email = "pecb-test-user3@pecb.com", Department = Department.Technical, IsActive = false });
}
