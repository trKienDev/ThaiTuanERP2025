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
	public class CashOutGroupConfiguration : IEntityTypeConfiguration<CashOutGroup>
	{
		public void Configure(EntityTypeBuilder<CashOutGroup> builder)
		{
			builder.ToTable("CashOutGroups", "Finance").HasKey(x => x.Id);
			
			builder.Property(x => x.Code).IsRequired().HasMaxLength(64);
			builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
			builder.Property(x => x.Description).HasMaxLength(1000);

			builder.HasIndex(x => x.Code).IsUnique();
			builder.HasIndex(x => x.Name);
		}
	}
}
