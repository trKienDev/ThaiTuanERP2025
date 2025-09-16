using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Infrastructure.Account.Configurations
{
	public class DivisionConfiguration : IEntityTypeConfiguration<Division>
	{
		public void Configure(EntityTypeBuilder<Division> builder)
		{
			builder.ToTable("Divisions", "Core");
			builder.HasKey(d => d.Id);
			builder.Property(d => d.Name).IsRequired().HasMaxLength(256);
			builder.Property(d => d.Description).HasMaxLength(1024);
			
			builder.HasIndex(x => x.Name).IsUnique();
			builder.HasOne(d => d.HeadUser)
				   .WithMany()
				   .HasForeignKey(d => d.HeadUserId)
				   .OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(e => e.CreatedByUser)
				.WithMany()
				.HasForeignKey(e => e.CreatedByUserId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(e => e.ModifiedByUser)
				.WithMany()
				.HasForeignKey(e => e.ModifiedByUserId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(e => e.DeletedByUser)
				.WithMany()
				.HasForeignKey(e => e.DeletedByUserId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasIndex(e => e.CreatedByUserId);
			builder.HasIndex(e => e.ModifiedByUserId);
			builder.HasIndex(e => e.DeletedByUserId);
		}
	}
}
