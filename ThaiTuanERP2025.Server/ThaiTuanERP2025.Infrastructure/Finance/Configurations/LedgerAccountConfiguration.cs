using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence.Configurations;

namespace ThaiTuanERP2025.Infrastructure.Finance.Configurations
{
	public class LedgerAccountConfiguration : BaseEntityConfiguration<LedgerAccount>
	{
		public override void Configure(EntityTypeBuilder<LedgerAccount> builder)
		{
			builder.ToTable("LedgerAccounts", "Finance").HasKey(x => x.Id);

			builder.Property(x => x.Number).IsRequired().HasMaxLength(50);
			builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
			builder.Property(x => x.Description).HasMaxLength(500);
			builder.Property(x => x.Path).IsRequired().HasMaxLength(500);
			builder.Property(x => x.Level).HasDefaultValue(0);
			builder.Property(x => x.IsActive).HasDefaultValue(true);
			builder.Property(x => x.LedgerAccountBalanceType).HasConversion<int>().IsRequired();

			// Quan hệ cha-con
			builder.HasOne(x => x.Parent)
				.WithMany(x => x.Children)
				.HasForeignKey(x => x.ParentLedgerAccountId)
				.OnDelete(DeleteBehavior.Restrict);

			// Quan hệ với LedgerAccountType
			builder.HasOne(x => x.LedgerAccountType)
				.WithMany(x => x.LedgerAccounts)
				.HasForeignKey(x => x.LedgerAccountTypeId)
				.OnDelete(DeleteBehavior.Restrict);

			// ✅ Set Field Access Mode nếu tồn tại
			var nav = builder.Metadata.FindNavigation(nameof(LedgerAccount.Children));
			if (nav != null)
				nav.SetPropertyAccessMode(PropertyAccessMode.Field);

			builder.HasIndex(x => x.Number).IsUnique();
			builder.HasIndex(x => new { x.Number, x.Name  }).IsUnique();
			builder.HasIndex(x => x.IsActive);
			builder.HasIndex(x => x.Path);
			builder.HasIndex(x => new { x.Number, x.IsDeleted });

			// Auditable
			ConfigureAuditUsers(builder);
		}
	}
}
