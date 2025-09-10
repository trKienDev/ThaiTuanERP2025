using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Infrastructure.Finance.Configurations
{
	public class CashOutCodeConfiguration : IEntityTypeConfiguration<CashoutCode>
	{
		public void Configure(EntityTypeBuilder<CashoutCode> builder)
		{
			builder.ToTable("CashoutCodes", "Finance").HasIndex(x => x.Id);

			builder.Property(x => x.Code).IsRequired().HasMaxLength(64);
			builder.Property(x => x.Name).IsRequired().HasMaxLength(250);
			builder.Property(x => x.Description).HasMaxLength(1000);

			builder.HasIndex(x => x.Code).IsUnique();
			builder.HasIndex(x => x.Name);

			builder.HasOne(x => x.CashoutGroup)
				.WithMany(x => x.CashoutCodes)
				.HasForeignKey(x => x.CashoutGroupId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(x => x.PostingLedgerAccount)
				.WithOne(a => a.CashoutCode)
				.HasForeignKey<CashoutCode>(x => x.PostingLedgerAccountId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasIndex(x => x.PostingLedgerAccountId).IsUnique();
		}
	}
}
