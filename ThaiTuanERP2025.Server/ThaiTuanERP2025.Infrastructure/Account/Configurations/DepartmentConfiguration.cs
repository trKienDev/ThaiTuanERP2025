using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence.Configurations;

namespace ThaiTuanERP2025.Infrastructure.Account.Configurations
{
	public class DepartmentConfiguration : BaseEntityConfiguration<Department>
	{
		public override void Configure(EntityTypeBuilder<Department> builder)
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

			builder.HasMany<User>("_users")
				.WithOne()
				.HasForeignKey("DepartmentId")
				.OnDelete(DeleteBehavior.Restrict);

			// Indexes
			builder.HasIndex(e => e.CreatedByUserId);
			builder.HasIndex(e => e.ModifiedByUserId);
			builder.HasIndex(e => e.DeletedByUserId);

			// Private collections
			builder.HasMany(d => d.Users)
				.WithOne(u => u.Department)
				.HasForeignKey(u => u.DepartmentId)
				.OnDelete(DeleteBehavior.Restrict);
			
		     builder.Metadata.FindNavigation(nameof(Department.Users))!
			    .SetPropertyAccessMode(PropertyAccessMode.Field);

			builder.Metadata.FindNavigation(nameof(Department.Children))!
				.SetPropertyAccessMode(PropertyAccessMode.Field);

			// Indexes
			builder.HasIndex(d => d.Code).IsUnique();
		}
	}
}
