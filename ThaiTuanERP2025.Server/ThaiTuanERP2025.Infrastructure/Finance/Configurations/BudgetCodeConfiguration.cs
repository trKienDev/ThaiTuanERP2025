using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Infrastructure.Finance.Configurations
{
	public class BudgetCodeConfiguration : IEntityTypeConfiguration<BudgetCode>
	{
		public void Configure(EntityTypeBuilder<BudgetCode> builder)
		{
			builder.ToTable("BudgetCode", "Finance").HasIndex(x => x.Id);

			builder.HasIndex(e => e.Code).IsUnique();
			builder.Property(e => e.Code).IsRequired().HasMaxLength(50);
			builder.Property(e => e.Name).IsRequired().HasMaxLength(255);

			builder.HasOne(e => e.BudgetGroup)
				.WithMany(g => g.BudgetCodes)
				.HasForeignKey(e => e.BudgetGroupId)
				.OnDelete(DeleteBehavior.Restrict);
			builder.HasIndex(bc => new { bc.BudgetGroupId, bc.Code }).IsUnique();

			builder.HasOne(e => e.CashoutCode)
				.WithMany(c => c.BudgetCodes)
				.HasForeignKey(e => e.CashoutCodeId)
				.OnDelete(DeleteBehavior.Restrict);
			builder.HasIndex(e => e.CashoutCodeId);

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
