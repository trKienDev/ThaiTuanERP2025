using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Authentication.Entities;

namespace ThaiTuanERP2025.Infrastructure.Authentication.Configurations
{
	public sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
	{
		public void Configure(EntityTypeBuilder<RefreshToken> builder)
		{
			builder.ToTable("RefreshTokens", "Auth");

			builder.HasKey(x => x.Id);

			builder.Property(x => x.TokenHash)
				.IsRequired()
				.HasMaxLength(128);

			builder.Property(x => x.ReplacedByTokenHash)
				.HasMaxLength(128);

			builder.Property(x => x.CreatedByIp).HasMaxLength(64);
			builder.Property(x => x.RevokedByIp).HasMaxLength(64);

			builder.HasIndex(x => x.TokenHash).IsUnique();
			builder.HasIndex(x => new { x.UserId, x.IsRevoked, x.ExpiresAt });
		}
	}
}
