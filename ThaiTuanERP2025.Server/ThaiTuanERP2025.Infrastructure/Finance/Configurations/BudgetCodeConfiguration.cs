using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence.Configurations;

namespace ThaiTuanERP2025.Infrastructure.Finance.Configurations
{
	public class BudgetCodeConfiguration : BaseEntityConfiguration<BudgetCode>
	{
		public override void Configure(EntityTypeBuilder<BudgetCode> builder)
		{
			builder.ToTable("BudgetCode", "Finance").HasIndex(x => x.Id);

			// ===== Basic properties =====
			builder.Property(x => x.Code).HasMaxLength(50).IsRequired();
			builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
			builder.Property(x => x.IsActive).IsRequired();
			builder.Property(x => x.CashoutCodeId).IsRequired(false);

			// ===== Relationships =====

			// BudgetGroup (1 - n)
			builder.HasOne(x => x.BudgetGroup)
			    .WithMany(x => x.BudgetCodes)
			    .HasForeignKey(x => x.BudgetGroupId)
			    .OnDelete(DeleteBehavior.Cascade);

			// CashoutCode (1 - n)
			builder.HasOne(x => x.CashoutCode)
				.WithMany()
				.HasForeignKey(x => x.CashoutCodeId)
				.OnDelete(DeleteBehavior.SetNull);

			// ===== Indexes & Constraints =====
			builder.HasIndex(x => new { x.BudgetGroupId, x.Code }).IsUnique(); // Code duy nhất trong mỗi nhóm
			builder.HasIndex(x => x.IsActive);
			builder.HasIndex(x => x.Name);

			// Auditable
			ConfigureAuditUsers(builder);
		}
	}
}
