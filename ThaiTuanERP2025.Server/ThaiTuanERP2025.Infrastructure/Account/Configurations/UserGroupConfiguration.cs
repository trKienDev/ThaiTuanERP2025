using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Infrastructure.Account.Configurations
{
	public class UserGroupConfiguration : IEntityTypeConfiguration<UserGroup>
	{
		public void Configure(EntityTypeBuilder<UserGroup> builder)
		{
			builder.ToTable("UserGroups", "Account");

			builder.HasKey(ug => ug.Id);

			builder.Property(ug => ug.JoinedAt).IsRequired();

			builder.Property(ug => ug.IsActive).HasDefaultValue(true);

			// Relations
			builder.HasOne(ug => ug.User)
				.WithMany(u => u.UserGroups)
				.HasForeignKey(ug => ug.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(ug => ug.Group)
				.WithMany(g => g.UserGroups)
				.HasForeignKey(ug => ug.GroupId)
				.OnDelete(DeleteBehavior.Cascade);

			// Audit (CreatedByUser, etc.)
			builder.HasOne(ug => ug.CreatedByUser)
				.WithMany()
				.HasForeignKey("CreatedByUserId")
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(ug => ug.ModifiedByUser)
				.WithMany()
				.HasForeignKey("ModifiedByUserId")
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(ug => ug.DeletedByUser)
				.WithMany()
				.HasForeignKey("DeletedByUserId")
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasIndex(ug => new { ug.UserId, ug.GroupId }).IsUnique();
		}
	}
}
