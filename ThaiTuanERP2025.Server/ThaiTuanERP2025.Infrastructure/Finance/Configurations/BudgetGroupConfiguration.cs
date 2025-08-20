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
	public class BudgetGroupConfiguration : IEntityTypeConfiguration<BudgetGroup>
	{
		public void Configure(EntityTypeBuilder<BudgetGroup> builder) {
			builder.ToTable("BudgetGroup", "Finance");
			builder.HasKey(x => x.Id);

			builder.HasIndex(e => e.Code).IsUnique();
			builder.Property(e => e.Code).IsRequired().HasMaxLength(50);
			builder.Property(e => e.Name).IsRequired().HasMaxLength(255);

			// Audit User
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
