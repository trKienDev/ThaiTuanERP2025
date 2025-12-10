using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Core.Entities;

namespace ThaiTuanERP2025.Infrastructure.Core.Configurations
{
	public sealed class CommentMentionConfiguration : IEntityTypeConfiguration<CommentMention>
	{
		public void Configure(EntityTypeBuilder<CommentMention> builder)
		{
			builder.ToTable("CommentMentions", "Core");

			builder.HasKey(x => x.Id);

			builder.Property(x => x.CommentId).IsRequired();
			builder.Property(x => x.UserId).IsRequired();

			builder.HasIndex(x => new { x.CommentId, x.UserId }).IsUnique();

			builder.HasOne(m => m.Comment)
				.WithMany(c => c.Mentions)
				.HasForeignKey(m => m.CommentId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(m => m.User)
				.WithMany()
				.HasForeignKey(m => m.UserId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
