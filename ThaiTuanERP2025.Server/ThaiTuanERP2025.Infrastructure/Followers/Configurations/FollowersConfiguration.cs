using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Followers.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence.Configurations;

namespace ThaiTuanERP2025.Infrastructure.Followers.Configurations
{
	public sealed class FollowerConfiguration : BaseEntityConfiguration<Follower>
	{
		public override void Configure(EntityTypeBuilder<Follower> builder)
		{
			builder.ToTable("Followers", "Core");

			builder.HasKey(f => f.Id);

			// ===== Value Object: SubjectRef =====
			builder.OwnsOne(x => x.Subject, subject =>
			{
				subject.Property(s => s.Type).HasConversion<int>().HasColumnName("Subject_Type").IsRequired();
				subject.Property(s => s.Id).HasColumnName("Subject_Id").IsRequired();
				subject.WithOwner();
			});

			// ===== Properties =====
			builder.Property(x => x.UserId).IsRequired();
			builder.Property(x => x.IsActive).IsRequired();

			// ===== Auditable =====
			ConfigureAuditUsers(builder);
		}
	}
}
