using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Infrastructure.Finance.Configurations
{
	public class BudgetPlanConfiguration : IEntityTypeConfiguration<BudgetPlan>
	{
		public void Configure(EntityTypeBuilder<BudgetPlan> builder)
		{
			builder.ToTable("BudgetPlan", "Finance");

			builder.HasKey(e => e.Id);
			builder.Property(e => e.Amount).HasColumnType("decimal(18, 2)").IsRequired();
			builder.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
			builder.Property(e => e.RowVersion).IsRowVersion();
			builder.HasIndex(e => new { e.DepartmentId, e.BudgetCodeId, e.BudgetPeriodId }).IsUnique();

			builder.HasOne(e => e.Department)
				.WithMany()
				.HasForeignKey(e => e.DepartmentId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(e => e.BudgetCode)
				.WithMany(c => c.BudgetPlans)
				.HasForeignKey(e => e.BudgetCodeId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(e => e.BudgetPeriod)
				.WithMany(p => p.BudgetPlans)
				.HasForeignKey(e => e.BudgetPeriodId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasMany(e => e.Transactions)
				.WithOne(t => t.BudgetPlan)
				.HasForeignKey(t => t.BudgetPlanId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(e => e.ReviewedByUser)
				.WithMany()
				.HasForeignKey(e => e.ReviewedByUserId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(e => e.ApprovedByUser)
				.WithMany()
				.HasForeignKey(e => e.ApprovedByUserId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(e => e.BudgetCode)
				.WithMany(c => c.BudgetPlans)
				.HasForeignKey(e => e.BudgetCodeId)
				.OnDelete(DeleteBehavior.Restrict);
			builder.HasOne(e => e.BudgetPeriod)
				.WithMany(p => p.BudgetPlans)
				.HasForeignKey(e => e.BudgetPeriodId)
				.OnDelete(DeleteBehavior.Restrict);

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
