using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence.Configurations;

namespace ThaiTuanERP2025.Infrastructure.Finance.Configurations
{
	public class BudgetGroupConfiguration : BaseEntityConfiguration<BudgetGroup>
	{
		public override void Configure(EntityTypeBuilder<BudgetGroup> builder)
		{
			builder.ToTable("BudgetGroup", "Finance");
			builder.HasKey(x => x.Id);

			// ===== Basic properties =====
			builder.Property(x => x.Code).HasMaxLength(50).IsRequired();

			builder.Property(x => x.Name).HasMaxLength(200).IsRequired();

			// ===== Navigation BudgetCodes (private field) =====
			builder.Navigation(nameof(BudgetGroup.BudgetCodes))
				.UsePropertyAccessMode(PropertyAccessMode.Field); // EF truy cập private field

			builder.HasMany(typeof(BudgetCode), nameof(BudgetGroup.BudgetCodes))
				.WithOne(nameof(BudgetCode.BudgetGroup))
				.HasForeignKey(nameof(BudgetCode.BudgetGroupId))
				.OnDelete(DeleteBehavior.Cascade); // Khi xóa nhóm, xóa luôn các code thuộc nhóm đó

			// ===== Indexes =====
			builder.HasIndex(x => x.Code).IsUnique(); // Mã nhóm ngân sách phải duy nhất
			builder.HasIndex(x => x.Name);

			// Auditable
			ConfigureAuditUsers(builder);
		}
	}
}
