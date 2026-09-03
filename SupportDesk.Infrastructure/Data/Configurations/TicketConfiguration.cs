using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupportDesk.Core.Entities;

namespace SupportDesk.Infrastructure.Data.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
	public void Configure(EntityTypeBuilder<Ticket> builder)
	{
		builder.HasKey(t => t.Id);

		builder.Property(t => t.Reference)
			   .IsRequired()
			   .HasMaxLength(20);

		builder.HasIndex(t => t.Reference)
			   .IsUnique();

		builder.Property(t => t.Title)
			   .IsRequired()
			   .HasMaxLength(200);

		builder.Property(t => t.Description)
			   .IsRequired()
			   .HasMaxLength(4000);

		builder.Property(t => t.CustomerName)
			   .IsRequired()
			   .HasMaxLength(200);

		builder.Property(t => t.CustomerEmail)
			   .IsRequired()
			   .HasMaxLength(256);

		builder.Property(t => t.Priority)
			   .IsRequired()
			   .HasConversion<string>()
			   .HasMaxLength(20);

		builder.Property(t => t.Status)
			   .IsRequired()
			   .HasConversion<string>()
			   .HasMaxLength(20);

		builder.Property(t => t.CreatedDate)
			   .IsRequired();

		builder.Property(t => t.LastModifiedDate)
			   .IsRequired();

		builder.Property(t => t.DueDate)
			   .IsRequired();

		builder.Ignore(t => t.IsOverdue);

		builder.HasIndex(t => t.Status);
		builder.HasIndex(t => t.Priority);
		builder.HasIndex(t => t.DueDate);

		#region Relationships
		builder.HasOne(t => t.AssignedAgent)
			   .WithMany(a => a.Tickets)
			   .HasForeignKey(t => t.AssignedAgentId)
			   .OnDelete(DeleteBehavior.Restrict)
			   .IsRequired(false);
		#endregion
	}
}
