using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence.Configurations;

namespace ThaiTuanERP2025.Infrastructure.Core.Configurations
{
	public sealed class FollowerConfiguration : BaseEntityConfiguration<Follower>
	{
		public override void Configure(EntityTypeBuilder<Follower> builder)
		{
			builder.ToTable("Followers", "Core");

			builder.HasKey(f => f.Id);

			// ===== Properties =====
			builder.Property(x => x.SubjectId).IsRequired();
			builder.Property(x => x.SubjectType).IsRequired().HasConversion<int>();
			builder.Property(x => x.UserId).IsRequired();
			builder.Property(x => x.IsActive).IsRequired();

			// ===== Auditable =====
			ConfigureAuditUsers(builder);
		}
	}
}
