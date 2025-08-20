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
	public class TaxConfiguration : IEntityTypeConfiguration<Tax>
	{
		public void Configure(EntityTypeBuilder<Tax> builder)
		{
			builder.ToTable("Taxes", "Finance").HasKey(x => x.Id);

			builder.Property(x => x.PolicyName).IsRequired().HasMaxLength(200);
			builder.Property(x => x.Rate).IsRequired().HasColumnType("decimal(5,2)");
			builder.Property(x => x.Description).HasMaxLength(1000);

			builder.HasIndex(x => x.PolicyName).IsUnique();

			builder.HasOne(x => x.PostingLedgerAccount)
				.WithMany()
				.HasForeignKey(x => x.PostingLedgerAccountId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
