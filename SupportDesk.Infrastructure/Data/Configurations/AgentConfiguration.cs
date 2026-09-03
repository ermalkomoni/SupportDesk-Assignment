using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupportDesk.Core.Entities;

namespace SupportDesk.Infrastructure.Data.Configurations;

public class AgentConfiguration : IEntityTypeConfiguration<Agent>
{
	public void Configure(EntityTypeBuilder<Agent> builder)
	{
		builder.HasKey(a => a.Id);

		builder.Property(a => a.FullName)
			   .IsRequired()
			   .HasMaxLength(200);

		builder.Property(a => a.Email)
			   .IsRequired()
			   .HasMaxLength(256);

		builder.HasIndex(a => a.Email)
			   .IsUnique();

		builder.Property(a => a.Department)
			   .IsRequired()
			   .HasConversion<string>()
			   .HasMaxLength(20);

		builder.Property(a => a.IsActive)
			   .IsRequired();

	}
}
