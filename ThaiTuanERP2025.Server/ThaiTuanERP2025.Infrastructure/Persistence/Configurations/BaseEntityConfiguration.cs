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

		protected void ConfigureAuditUsers(EntityTypeBuilder<TEntity> builder)
		{
			builder.HasOne(e => e.CreatedByUser)
			    .WithMany()
			    .HasForeignKey(e => e.CreatedByUserId)
			    .OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(e => e.ModifiedByUser)
			    .WithMany()
			    .HasForeignKey(e => e.ModifiedByUserId)
			    .OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(e => e.DeletedByUser)
			    .WithMany()
			    .HasForeignKey(e => e.DeletedByUserId)
			    .OnDelete(DeleteBehavior.Restrict);
		}
	}


}
