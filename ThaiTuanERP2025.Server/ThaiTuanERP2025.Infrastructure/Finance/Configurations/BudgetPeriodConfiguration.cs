using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Infrastructure.Finance.Configurations
{
	public class BudgetPeriodConfiguration : IEntityTypeConfiguration<BudgetPeriod>
	{
		public void Configure(EntityTypeBuilder<BudgetPeriod> builder)
		{
			builder.ToTable("BudgetPeriod", "Finance");

			builder.HasKey(e => e.Id);
			builder.HasIndex(e => new { e.Year, e.Month }).IsUnique();
			builder.Property(e => e.Year).IsRequired();
			builder.Property(e => e.Month).IsRequired();

			builder.HasOne(e => e.CreatedByUser)
				.WithMany()
				.HasForeignKey(e => e.CreatedByUserId)
				.OnDelete(DeleteBehavior.Restrict);
			builder.HasIndex(e => e.CreatedByUserId);

			builder.HasOne(e => e.ModifiedByUser)
				.WithMany()
				.HasForeignKey(e => e.ModifiedByUserId)
				.OnDelete(DeleteBehavior.Restrict);
			builder.HasIndex(e => e.ModifiedByUserId);

			builder.HasOne(e => e.DeletedByUser)
				.WithMany()
				.HasForeignKey(e => e.DeletedByUserId)
				.OnDelete(DeleteBehavior.Restrict);
			builder.HasIndex(e => e.DeletedByUserId);
		}
	}
}
