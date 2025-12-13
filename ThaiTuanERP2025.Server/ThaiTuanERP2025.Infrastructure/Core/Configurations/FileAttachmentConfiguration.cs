using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence.Configurations;

namespace ThaiTuanERP2025.Infrastructure.Core.Configurations
{
	public class FileAttachmentConfiguration : BaseEntityConfiguration<FileAttachment>
	{
		public override void Configure(EntityTypeBuilder<FileAttachment> builder)
		{
			builder.ToTable("FileAttachment", "Core");
			builder.HasKey(x => x.Id);

			// ====== Basic Properties ======
			builder.Property(x => x.Id).ValueGeneratedNever();
			builder.Property(x => x.DriveObjectId).IsRequired();
			builder.Property(x => x.Module).HasMaxLength(100).IsRequired();
			builder.Property(x => x.Document).HasMaxLength(100).IsRequired();
			builder.Property(x => x.DocumentId).HasMaxLength(64);

			// ====== Indexes ======
			builder.HasIndex(x => new { x.Module, x.Document, x.DocumentId });
			builder.HasIndex(x => x.DriveObjectId);

			// Auditable
			ConfigureAuditUsers(builder);
		}
	}
}
