using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Files.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence.Configurations;

namespace ThaiTuanERP2025.Infrastructure.StoredFiles.Configurations
{
	public class StoredFileConfiguration : BaseEntityConfiguration<StoredFile>
	{
		public override void Configure(EntityTypeBuilder<StoredFile> builder)
		{
			builder.ToTable("StoredFiles", "Files");
			builder.HasKey(x => x.Id);

			// ====== Basic Properties ======
			builder.Property(x => x.Bucket).HasMaxLength(200).IsRequired();
			builder.Property(x => x.ObjectKey).HasMaxLength(500).IsRequired();
			builder.Property(x => x.FileName).HasMaxLength(255).IsRequired();
			builder.Property(x => x.ContentType).HasMaxLength(150).IsRequired();
			builder.Property(x => x.Size).HasColumnType("bigint").IsRequired();
			builder.Property(x => x.Hash).HasMaxLength(256);
			builder.Property(x => x.Module).HasMaxLength(100).IsRequired();
			builder.Property(x => x.Entity).HasMaxLength(100).IsRequired();
			builder.Property(x => x.EntityId).HasMaxLength(64);
			builder.Property(x => x.IsPublic).IsRequired();

			// ====== Indexes ======
			builder.HasIndex(x => new { x.Bucket, x.ObjectKey }).IsUnique(); // objectKey unique trong bucket
			builder.HasIndex(x => new { x.Module, x.Entity, x.EntityId });
			builder.HasIndex(x => x.IsPublic);

			// Auditable
			ConfigureAuditUsers(builder);
		}
	}
}
