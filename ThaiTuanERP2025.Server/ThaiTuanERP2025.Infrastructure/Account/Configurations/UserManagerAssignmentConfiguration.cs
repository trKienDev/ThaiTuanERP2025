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

			builder.HasIndex(x => new { x.UserId, x.ManagerId })
				.IsUnique().HasFilter("[IsActive] = 1 AND [IsDeleted] = 0"); ;
			
			builder.HasIndex(x => x.UserId)
				.IsUnique().HasFilter("[IsActive] = 1 AND [IsPrimary] = 1");

			ConfigureAuditUsers(builder);
		}
	}
}