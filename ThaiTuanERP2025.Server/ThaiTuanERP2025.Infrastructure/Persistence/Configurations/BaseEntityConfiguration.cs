using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Infrastructure.Persistence.Configurations
{
	/// <summary>
	/// Base class hỗ trợ cấu hình 3 quan hệ audit: CreatedByUser, ModifiedByUser, DeletedByUser.
	/// </summary>
	public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class
	{
		public abstract void Configure(EntityTypeBuilder<TEntity> builder);

		/// <summary>
		/// Cấu hình quan hệ audit user (CreatedBy, ModifiedBy, DeletedBy)
		/// </summary>
		protected void ConfigureAuditUsers(EntityTypeBuilder<TEntity> builder)
		{
			builder.HasOne<User>()
				.WithMany()
				.HasForeignKey("CreatedByUserId")
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne<User>()
				.WithMany()
				.HasForeignKey("ModifiedByUserId")
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne<User>()
				.WithMany()
				.HasForeignKey("DeletedByUserId")
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
