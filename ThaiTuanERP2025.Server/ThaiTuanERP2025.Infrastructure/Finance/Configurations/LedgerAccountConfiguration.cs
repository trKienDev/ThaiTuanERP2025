using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Infrastructure.Finance.Configurations
{
	public class LedgerAccountConfiguration : IEntityTypeConfiguration<LedgerAccount>
	{
		public void Configure(EntityTypeBuilder<LedgerAccount> builder)
		{
			builder.ToTable("LedgerAccounts", "Finance").HasKey(x => x.Id);

			builder.Property(x => x.Number).IsRequired().HasMaxLength(50);
			builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
			builder.Property(x => x.Description).HasMaxLength(1000);

			builder.Property(x => x.Path).IsRequired().HasMaxLength(900);
			builder.Property(x => x.Level).IsRequired();

			builder.HasIndex(x => x.Number).IsUnique();
			builder.HasIndex(x => x.Path);
			builder.HasIndex(x => x.ParentLedgerAccountId);

			builder.HasOne(x => x.LedgerAccountType)
				.WithMany(x => x.LedgerAccounts)
				.HasForeignKey(x => x.LedgerAccountTypeId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(x => x.Parent)
				.WithMany(x => x.Children)
				.HasForeignKey(x => x.ParentLedgerAccountId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
