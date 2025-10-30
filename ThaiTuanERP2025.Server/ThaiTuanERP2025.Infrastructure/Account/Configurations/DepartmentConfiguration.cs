using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Infrastructure.Account.Configurations
{
	public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
	{
		public void Configure(EntityTypeBuilder<Department> builder)
		{
			builder.ToTable("Departments", "Account");

			builder.HasKey(d => d.Id);

			builder.Property(d => d.Name).IsRequired().HasMaxLength(150);
			builder.Property(d => d.Code).IsRequired().HasMaxLength(50);
			builder.Property(d => d.Level).HasDefaultValue(0);
			builder.Property(d => d.IsActive).HasDefaultValue(true);

			// Self reference (Parent)
			builder.HasOne(d => d.Parent)
				.WithMany(p => p.Children)
				.HasForeignKey(d => d.ParentId)
				.OnDelete(DeleteBehavior.Restrict);

			// Manager (User)
			builder.HasOne<User>()
				.WithMany()
				.HasForeignKey(d => d.ManagerUserId)
				.OnDelete(DeleteBehavior.SetNull);

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

			builder.HasIndex(e => e.CreatedByUserId);
			builder.HasIndex(e => e.ModifiedByUserId);
			builder.HasIndex(e => e.DeletedByUserId);

			// Private collections
			builder.Metadata.FindNavigation(nameof(Department.Users))!
				.SetPropertyAccessMode(PropertyAccessMode.Field);

			builder.Metadata.FindNavigation(nameof(Department.Children))!
				.SetPropertyAccessMode(PropertyAccessMode.Field);

			// Indexes
			builder.HasIndex(d => d.Code).IsUnique();
		}
	}
}
