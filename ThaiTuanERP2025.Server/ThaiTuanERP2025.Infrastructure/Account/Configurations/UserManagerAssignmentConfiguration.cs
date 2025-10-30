using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Infrastructure.Account.Configurations
{
	public class UserManagerAssignmentConfiguration : IEntityTypeConfiguration<UserManagerAssignment>
	{
		public void Configure(EntityTypeBuilder<UserManagerAssignment> builder)
		{
			builder.ToTable("UserManagerAssignments", "Account");
			builder.HasKey(x => new { x.UserId, x.ManagerId });

			// FK: UserId -> User
			builder.HasOne(x => x.User)
				.WithMany(u => u.DirectReportsAssignments)
				.HasForeignKey(x => x.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			// FK: ManagerId -> User
			builder.HasOne(x => x.Manager)
				.WithMany(u => u.ManagerAssignments)
				.HasForeignKey(x => x.ManagerId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasIndex(x => new { x.UserId, x.IsPrimary })
				.IsUnique()
				.HasFilter("[IsPrimary] = 1");

			// Không tự quản lý chính mình
			builder.ToTable("UserManagerAssignments", "Core", t =>
			{
				t.HasCheckConstraint("CK_UserManagerAssignments_NoSelfManagement", "[UserId] <> [ManagerId]");
			});
		}
	}
}
