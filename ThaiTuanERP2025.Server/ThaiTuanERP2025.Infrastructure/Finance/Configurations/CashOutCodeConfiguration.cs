using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Infrastructure.Finance.Configurations
{
	public class CashOutCodeConfiguration : IEntityTypeConfiguration<CashOutCode>
	{
		public void Configure(EntityTypeBuilder<CashOutCode> builder)
		{
			builder.ToTable("CashOutCodes", "Finance").HasIndex(x => x.Id);

			builder.Property(x => x.Code).IsRequired().HasMaxLength(64);
			builder.Property(x => x.Name).IsRequired().HasMaxLength(250);
			builder.Property(x => x.Description).HasMaxLength(1000);

			builder.HasIndex(x => x.Code).IsUnique();
			builder.HasIndex(x => x.Name);

			builder.HasOne(x => x.CashOutGroup)
				.WithMany(x => x.CashOutCodes)
				.HasForeignKey(x => x.CashOutGroupId)
				.OnDelete(DeleteBehavior.Restrict);
			builder.HasOne(x => x.PostingLedgerAccount)
				.WithMany()
				.HasForeignKey(x => x.PostingLedegerAccoutnId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
