using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Core.Entities;

namespace ThaiTuanERP2025.Infrastructure.Core.Configurations
{
	public sealed class CommentAttachmentConfiguration : IEntityTypeConfiguration<CommentAttachment>
	{
		public void Configure(EntityTypeBuilder<CommentAttachment> builder)
		{
			builder.ToTable("CommentAttachments", "Core");
			builder.HasKey(x => x.Id);

			builder.Property(x => x.StoredFileId).IsRequired();

			builder.HasOne(x => x.StoredFile)
				.WithMany()
				.HasForeignKey(x => x.StoredFileId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasIndex(x => x.CommentId);
		}
	}
}
