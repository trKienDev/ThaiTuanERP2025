using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence.Configurations;

namespace ThaiTuanERP2025.Infrastructure.Finance.Configurations
{
	public class BudgetPeriodConfiguration :BaseEntityConfiguration<BudgetPeriod>
	{
		public override void Configure(EntityTypeBuilder<BudgetPeriod> builder)
		{
			builder.ToTable("BudgetPeriod", "Finance");

			builder.HasKey(e => e.Id);

			// ===== Basic properties =====
			builder.Property(x => x.Year).IsRequired();
			builder.Property(x => x.Month).IsRequired();
			builder.Property(x => x.StartDate).IsRequired();
			builder.Property(x => x.EndDate).IsRequired();
			builder.Property(x => x.IsActive).IsRequired();

			// ===== Navigation BudgetPlans =====
			builder.Navigation(nameof(BudgetPeriod.BudgetPlans))
				.UsePropertyAccessMode(PropertyAccessMode.Field);

			builder.HasMany(typeof(BudgetPlan), nameof(BudgetPeriod.BudgetPlans))
				.WithOne(nameof(BudgetPlan.BudgetPeriod))
				.HasForeignKey(nameof(BudgetPlan.BudgetPeriodId))
				.OnDelete(DeleteBehavior.Cascade);

			// ===== Indexes =====
			builder.HasIndex(x => new { x.Year, x.Month }).IsUnique();
			builder.HasIndex(x => x.IsActive);
		}
	}
}
