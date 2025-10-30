using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Infrastructure.Account.Configurations
{
	public class GroupConfiguration : IEntityTypeConfiguration<Group>
	{
		public void Configure(EntityTypeBuilder<Group> builder) {
			builder.ToTable("Group", "Account").HasKey(x => x.Id);
			builder.Property(g => g.Name).IsRequired().HasMaxLength(100);
			builder.Property(g => g.Description).HasMaxLength(255);
			builder.Property(g => g.Slug).IsRequired().HasMaxLength(100);
			builder.HasIndex(g => g.Slug).IsUnique();
			builder.ToTable(t => t.HasCheckConstraint("CK_Group_Slug_NoSpace", "CHARINDEX(' ', [Slug]) = 0"));

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
