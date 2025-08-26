using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Files.Entities;

namespace ThaiTuanERP2025.Infrastructure.StoredFiles.Configurations
{
	public class StoredFileConfiguration : IEntityTypeConfiguration<StoredFile>
	{
		public void Configure(EntityTypeBuilder<StoredFile> builder)
		{
			builder.ToTable("StoredFiles", "Files");
			builder.HasKey(x => x.Id);

			builder.Property(x => x.Bucket).HasMaxLength(128).IsRequired();
			builder.Property(x => x.ObjectKey).HasMaxLength(1024).IsRequired();
			builder.Property(x => x.FileName).HasMaxLength(255).IsRequired();
			builder.Property(x => x.ContentType).HasMaxLength(127).IsRequired();
			builder.Property(x => x.Module).HasMaxLength(64).IsRequired();
			builder.Property(x => x.Entity).HasMaxLength(64).IsRequired();
			builder.Property(x => x.EntityId).HasMaxLength(128);

			// Indexes
			builder.HasIndex(x => new { x.Bucket, x.ObjectKey }).IsUnique(); // tra cứu nhanh theo MinIO path
			builder.HasIndex(x => new { x.Module, x.Entity, x.EntityId });   // list theo chứng từ

			builder.HasQueryFilter(x => !x.IsDeleted);
		}
	}
}
