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

                        builder.Property(x => x.Module).HasMaxLength(64).IsRequired();
                        builder.Property(x => x.Entity).HasMaxLength(128).IsRequired();
                        builder.Property(x => x.EntityId).IsRequired();
                        builder.Property(x => x.UserId).IsRequired();

                        builder.HasOne(x => x.User)
                                .WithMany()
                                .HasForeignKey(x => x.UserId)
                                .OnDelete(DeleteBehavior.Restrict);

                        builder.HasOne(x => x.ParentComment)
                               .WithMany(x => x.Replies)
                               .HasForeignKey(x => x.ParentCommentId)
                               .OnDelete(DeleteBehavior.NoAction);

                        builder.HasIndex(x => new { x.Module, x.Entity, x.EntityId}).HasDatabaseName("IX_Comments_Module_Entity_EntityId");
			builder.HasIndex(x => x.UserId);

			builder.HasQueryFilter(x => !x.IsDeleted);

                        ConfigureAuditUsers(builder);
		}
	}
}
