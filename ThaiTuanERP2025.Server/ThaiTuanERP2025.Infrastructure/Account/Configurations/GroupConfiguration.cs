using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Infrastructure.Account.Configurations
{
	public class GroupConfiguration : IEntityTypeConfiguration<Group>
	{
		public void Configure(EntityTypeBuilder<Group> builder) {
			builder.ToTable("Group", "Core").HasKey(x => x.Id);
			builder.Property(g => g.Name).IsRequired().HasMaxLength(100);
			builder.Property(g => g.Description).HasMaxLength(255);

			builder.HasIndex(x => x.Name).IsUnique();

			builder.HasOne(e => e.CreatedByUser)
				.WithMany()
				.HasForeignKey(e => e.CreatedByUserId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(e => e.ModifiedByUser)
				.WithMany()
				.HasForeignKey(e => e.ModifiedByUserId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(e => e.DeletedByUser)
				.WithMany()
				.HasForeignKey(e => e.DeletedByUserId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasIndex(e => e.CreatedByUserId);
			builder.HasIndex(e => e.ModifiedByUserId);
			builder.HasIndex(e => e.DeletedByUserId);
		}
	}
}
