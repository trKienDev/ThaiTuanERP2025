using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.StoredFiles.Entities;

namespace Drive.Infrastructure.Persistence.Configurations
{

	public sealed class StoredObjectConfiguration : IEntityTypeConfiguration<StoredObject>
	{
		public void Configure(EntityTypeBuilder<StoredObject> builder)
		{
			builder.ToTable("StoredObject");

			builder.HasKey(x => x.Id);

			builder.Property(x => x.Bucket).IsRequired().HasMaxLength(100);
			builder.Property(x => x.ObjectKey).IsRequired().HasMaxLength(500);
			builder.Property(x => x.FileName).IsRequired().HasMaxLength(255);
			builder.Property(x => x.ContentType).IsRequired().HasMaxLength(100);
			builder.Property(x => x.Size).IsRequired();

			// Indexes (rất quan trọng cho lookup)
			builder.HasIndex(x => new { x.Bucket, x.ObjectKey }).IsUnique();
		}
	}
}
