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
	public class LedgerAccountTypeConfiguration : IEntityTypeConfiguration<LedgerAccountType>
	{
		public void Configure(EntityTypeBuilder<LedgerAccountType> builer) {
			builer.ToTable("LedgerAccountTypes", "Finance").HasKey(x => x.Id);
			builer.Property(x => x.Code).IsRequired().HasMaxLength(64);
			builer.Property(x => x.Name).IsRequired().HasMaxLength(250);
			builer.Property(x => x.Description).HasMaxLength(1000);
			builer.Property(x => x.IsActive).HasDefaultValue(true);
			builer.HasIndex(x => x.Code).IsUnique();
			builer.HasIndex(x => x.Name);
		}
	}
}
