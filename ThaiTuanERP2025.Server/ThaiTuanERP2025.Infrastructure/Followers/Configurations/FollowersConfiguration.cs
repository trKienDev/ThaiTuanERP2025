using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Followers.Entities;

namespace ThaiTuanERP2025.Infrastructure.Followers.Configurations
{
	public class FollowersConfiguration : IEntityTypeConfiguration<Follower>
	{
		public void Configure(EntityTypeBuilder<Follower> builder)
		{
			builder.ToTable("Followers", "Core");
			builder.HasKey(x => x.Id);

			builder.Property(x => x.SubjectType).HasConversion<string>().HasMaxLength(64).IsRequired();
			builder.Property(x => x.SubjectId).IsRequired();
			builder.Property(x => x.UserId).IsRequired();

			builder.HasIndex(x => new { x.SubjectType, x.SubjectId });
			builder.HasIndex(x => new { x.UserId, x.SubjectType, x.SubjectId }).IsUnique();
		}
	}
}
