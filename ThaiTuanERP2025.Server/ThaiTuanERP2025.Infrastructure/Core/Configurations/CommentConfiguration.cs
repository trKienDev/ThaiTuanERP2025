using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence.Configurations;

namespace ThaiTuanERP2025.Infrastructure.Core.Configurations
{
	public sealed class CommentConfiguration : BaseEntityConfiguration<Comment>
	{
                public override void Configure(EntityTypeBuilder<Comment> builder)
                {
                        builder.ToTable("Comments", "Core");
                        builder.HasKey(x => x.Id);

                        builder.Property(x => x.DocumentType).IsRequired().HasConversion<string>();
                        builder.Property(x => x.DocumentId).IsRequired();
                        builder.Property(x => x.UserId).IsRequired();

                        builder.HasOne(x => x.User)
                                .WithMany()
                                .HasForeignKey(x => x.UserId)
                                .OnDelete(DeleteBehavior.Restrict);

                        builder.HasOne(x => x.ParentComment)
                               .WithMany(x => x.Replies)
                               .HasForeignKey(x => x.ParentCommentId)
                               .OnDelete(DeleteBehavior.NoAction);

			builder.HasMany(c => c.Attachments)
	                        .WithOne(a => a.Comment)
	                        .HasForeignKey(a => a.CommentId)
	                        .OnDelete(DeleteBehavior.Cascade);

                        builder.HasMany(c => c.Mentions)
                                .WithOne(m => m.Comment)
                                .HasForeignKey(m => m.CommentId)
                                .OnDelete(DeleteBehavior.Cascade);

			builder.HasIndex(x => new { x.DocumentType, x.DocumentId}).HasDatabaseName("IX_Comments_DocumentType_DocumentId");
			builder.HasIndex(x => x.UserId);

			builder.HasQueryFilter(x => !x.IsDeleted);

                        ConfigureAuditUsers(builder);
		}
	}
}
