using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence.Configurations;

namespace ThaiTuanERP2025.Infrastructure.Account.Configurations
{
	public class GroupConfiguration : BaseEntityConfiguration<Group>
	{
		public override void Configure(EntityTypeBuilder<Group> builder)
		{
			builder.ToTable("Groups", "Account");

			builder.HasKey(g => g.Id);

			builder.Property(g => g.Name).IsRequired().HasMaxLength(150);

			builder.Property(g => g.Slug).IsRequired().HasMaxLength(100);

			builder.Property(g => g.Description).HasMaxLength(500);

			builder.Property(g => g.IsActive).HasDefaultValue(true);

			// Relationships
			builder.HasOne<User>()
				.WithMany()
				.HasForeignKey(g => g.AdminId)
				.OnDelete(DeleteBehavior.Restrict);

			// Private collection
			builder.Metadata.FindNavigation(nameof(Group.UserGroups))!
				.SetPropertyAccessMode(PropertyAccessMode.Field);

			builder.HasIndex(g => g.Slug).IsUnique();
		}
	}
}
