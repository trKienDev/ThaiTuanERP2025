using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence.Configurations;

namespace ThaiTuanERP2025.Infrastructure.Finance.Configurations
{
	//public class BudgetPlanConfiguration : BaseEntityConfiguration<BudgetPlan>
	//{
	//	public override void Configure(EntityTypeBuilder<BudgetPlan> builder)
	//	{
	//		builder.ToTable("BudgetPlan", "Finance");

	//		builder.HasKey(e => e.Id);

	//		// ===== Basic properties =====
	//		builder.Property(x => x.Amount).HasColumnType("decimal(18,2)").IsRequired();
	//		builder.Property(x => x.IsActive).IsRequired();
	//		builder.Property(x => x.Status).HasConversion<int>().IsRequired();
	//		builder.Property(x => x.RowVersion).IsRowVersion(); // concurrency token

	//		// ===== Relationships =====
	//		builder.HasOne(x => x.BudgetCode)
	//			.WithMany(x => x.BudgetPlans)
	//			.HasForeignKey(x => x.BudgetCodeId)
	//			.OnDelete(DeleteBehavior.Restrict);

	//		builder.HasOne(x => x.BudgetPeriod)
	//			.WithMany(x => x.BudgetPlans)
	//			.HasForeignKey(x => x.BudgetPeriodId)
	//			.OnDelete(DeleteBehavior.Cascade);

	//		builder.HasOne(x => x.Department)
	//			.WithMany()
	//			.HasForeignKey(x => x.DepartmentId)
	//			.OnDelete(DeleteBehavior.Restrict);

	//		// Review & Approval Users
	//		builder.HasOne(x => x.ReviewedByUser)
	//			.WithMany()
	//			.HasForeignKey(x => x.ReviewedByUserId)
	//			.OnDelete(DeleteBehavior.Restrict);

	//		builder.HasOne(x => x.ApprovedByUser)
	//			.WithMany()
	//			.HasForeignKey(x => x.ApprovedByUserId)
	//			.OnDelete(DeleteBehavior.Restrict);

	//		// Audit Users
	//		builder.HasOne(x => x.CreatedByUser)
	//			.WithMany()
	//			.HasForeignKey("CreatedByUserId")
	//			.OnDelete(DeleteBehavior.Restrict);

	//		builder.HasOne(x => x.ModifiedByUser)
	//			.WithMany()
	//			.HasForeignKey("ModifiedByUserId")
	//			.OnDelete(DeleteBehavior.Restrict);

	//		builder.HasOne(x => x.DeletedByUser)
	//			.WithMany()
	//			.HasForeignKey("DeletedByUserId")
	//			.OnDelete(DeleteBehavior.Restrict);

	//		// Transactions (private field)
	//		builder.Navigation(nameof(BudgetPlan.Transactions))
	//			.UsePropertyAccessMode(PropertyAccessMode.Field);

	//		builder.HasMany(typeof(BudgetTransaction), "_transactions")
	//			.WithOne(nameof(BudgetTransaction.BudgetPlan))
	//			.HasForeignKey(nameof(BudgetTransaction.BudgetPlanId))
	//			.OnDelete(DeleteBehavior.Cascade);

	//		// ===== Indexes =====
	//		builder.HasIndex(x => new { x.DepartmentId, x.BudgetCodeId, x.BudgetPeriodId }).IsUnique(); // Không cho phép trùng kế hoạch cùng mã ngân sách / phòng / kỳ

	//		builder.HasIndex(x => x.Status);
	//		builder.HasIndex(x => x.IsActive);
	//	}
	//}
}
