using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Core.Entities;

namespace ThaiTuanERP2025.Infrastructure.Core.Configurations
{
	public sealed class FollowerConfiguration : IEntityTypeConfiguration<Follower>
	{
		public void Configure(EntityTypeBuilder<Follower> builder)
		{
			builder.ToTable("Followers", "Core");

			// ===== Properties =====
			builder.Property(x => x.DocumentId).IsRequired();
			builder.HasKey(x => new { x.DocumentId, x.UserId });
			builder.Property(x => x.DocumentType).IsRequired().HasConversion<string>();
			builder.Property(x => x.UserId).IsRequired();
		}
	}
}
