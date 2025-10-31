using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence.Configurations;

namespace ThaiTuanERP2025.Infrastructure.Account.Configurations
{
	public class UserManagerAssignmentConfiguration : BaseEntityConfiguration<UserManagerAssignment>
	{
		public override void Configure(EntityTypeBuilder<UserManagerAssignment> builder)
		{
			builder.ToTable("UserManagerAssignments", "Account");

			builder.HasKey(x => x.Id);

			builder.Property(x => x.IsPrimary).IsRequired();
			builder.Property(x => x.IsActive).HasDefaultValue(true);
			builder.Property(x => x.AssignedAt).IsRequired();

			// Relations
			builder.HasOne(x => x.Manager)
				.WithMany(u => u.ManagerAssignments)
				.HasForeignKey(x => x.ManagerId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(x => x.User)
				.WithMany(u => u.DirectReportsAssignments)
				.HasForeignKey(x => x.UserId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasIndex(x => new { x.UserId, x.ManagerId }).IsUnique();

			ConfigureAuditUsers(builder);
		}
	}
}