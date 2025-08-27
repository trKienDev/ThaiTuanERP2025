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
	public class CashoutGroupConfiguration : IEntityTypeConfiguration<CashoutGroup>
	{
		public void Configure(EntityTypeBuilder<CashoutGroup> builder)
		{
			builder.ToTable("CashoutGroups", "Finance").HasKey(x => x.Id);
			
			builder.Property(x => x.Code).IsRequired().HasMaxLength(64);

			builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
			builder.Property(x => x.Description).HasMaxLength(1000);

			// Parent - Child reference
			builder.HasOne(x => x.Parent)
				.WithMany(x => x.Children)
				.HasForeignKey(x => x.ParentId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.Property(x => x.Level).HasDefaultValue(0);
			builder.Property(x => x.Path).HasMaxLength(1024);

			// Code là duy nhất trong phạm vi cùng Parent
			builder.HasIndex(x => new { x.ParentId, x.Code }).IsUnique();
			builder.HasIndex(x => x.Code).IsUnique();
			builder.HasIndex(x => x.Name);
		}
	}
}
