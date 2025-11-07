using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Domain.Common.Entities;

namespace ThaiTuanERP2025.Infrastructure.Persistence.Configurations
{
	/// <summary>
	/// Base class hỗ trợ cấu hình 3 quan hệ audit: CreatedByUser, ModifiedByUser, DeletedByUser.
	/// </summary>
	public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : AuditableEntity
	{
		public abstract void Configure(EntityTypeBuilder<TEntity> builder);

		protected static void ConfigureAuditUsers(EntityTypeBuilder<TEntity> builder)
		{
			builder.Property(e => e.CreatedByUserId).IsRequired(false);
			builder.Property(e => e.ModifiedByUserId).IsRequired(false);
			builder.Property(e => e.DeletedByUserId).IsRequired(false);

			builder.HasOne(e => e.CreatedByUser)
			    .WithMany()
			    .HasForeignKey(e => e.CreatedByUserId)
			    .OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(e => e.ModifiedByUser)
			    .WithMany()
			    .HasForeignKey(e => e.ModifiedByUserId)
			    .OnDelete(DeleteBehavior.NoAction);

			builder.HasOne(e => e.DeletedByUser)
			    .WithMany()
			    .HasForeignKey(e => e.DeletedByUserId)
			    .OnDelete(DeleteBehavior.NoAction);
		}
	}
}
