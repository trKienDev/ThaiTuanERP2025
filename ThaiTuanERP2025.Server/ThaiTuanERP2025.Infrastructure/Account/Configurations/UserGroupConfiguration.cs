using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence.Configurations;

namespace ThaiTuanERP2025.Infrastructure.Account.Configurations
{
	public class UserGroupConfiguration : BaseEntityConfiguration<UserGroup>
	{
		public override void Configure(EntityTypeBuilder<UserGroup> builder)
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

			builder.HasIndex(ug => new { ug.UserId, ug.GroupId }).IsUnique();

			ConfigureAuditUsers(builder);
		}
	}
}