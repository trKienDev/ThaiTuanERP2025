using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Infrastructure.Finance.Configurations
{
	public class BudgetPlanDetailConfiguration : IEntityTypeConfiguration<BudgetPlanDetail>
	{
		public void Configure(EntityTypeBuilder<BudgetPlanDetail> builder)
		{
			builder.ToTable("BudgetPlanDetails", "Finance");

			builder.HasKey(d => d.Id);

			// Decimal precision — quan trọng!
			builder.Property(d => d.Amount).HasPrecision(18, 2).IsRequired();
			builder.Property(d => d.IsActive).HasDefaultValue(true);
			builder.Property(d => d.ModifiedAt).IsRequired();
			builder.Property(d => d.DeletedAt).IsRequired(false);
			builder.Property(d => d.ModifiedByUserId).IsRequired();
			builder.Property(d => d.DeletedByUserId).IsRequired(false);

			// Relationship ModifiedByUser
			builder.HasOne(d => d.ModifiedByUser)
				.WithMany()
				.HasForeignKey(d => d.ModifiedByUserId)
				.OnDelete(DeleteBehavior.Restrict);

			// Relationship DeletedByUser
			builder.HasOne(d => d.DeletedByUser)
				.WithMany()
				.HasForeignKey(d => d.DeletedByUserId)
				.OnDelete(DeleteBehavior.Restrict);

			// Index để tìm chi tiết nhanh
			builder.HasIndex(d => d.BudgetPlanId);
			builder.HasIndex(d => d.BudgetCodeId);

			// đảm bảo không có 2 detail active với cùng BudgetCode trong 1 kế hoạch
			builder.HasIndex(d => new { d.BudgetPlanId, d.BudgetCodeId, d.IsActive }).IsUnique();
		}
	}
}
