using Microsoft.EntityFrameworkCore;
using SupportDesk.Core.Entities;
using SupportDesk.Infrastructure.Data.SeedData;

namespace SupportDesk.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
	{
	}

	public DbSet<Agent> Agents => Set<Agent>();
	public DbSet<Ticket> Tickets => Set<Ticket>();
	public DbSet<Comment> Comments => Set<Comment>();


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

		AgentSeedData.Seed(modelBuilder);
		TicketSeedData.Seed(modelBuilder);
	}
}