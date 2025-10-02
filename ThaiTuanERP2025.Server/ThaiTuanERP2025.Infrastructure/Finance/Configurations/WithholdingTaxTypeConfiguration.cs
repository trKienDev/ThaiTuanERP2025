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
	public class WithholdingTaxTypeConfiguration : IEntityTypeConfiguration<WithholdingTaxType>
	{
		public void Configure(EntityTypeBuilder<WithholdingTaxType> builder)
		{
			builder.ToTable("WithholdingTaxTypes", "Finance");
			builder.HasKey(w => w.Id);
			builder.Property(w => w.Name)
				.IsRequired()
				.HasMaxLength(100);
			builder.Property(w => w.Rate)
				.IsRequired()
				.HasColumnType("decimal(5, 2)");
			builder.Property(w => w.Description)
				.HasMaxLength(500);
			builder.Property(w => w.IsActive)
				.IsRequired()
				.HasDefaultValue(true);
		}
	}
}
