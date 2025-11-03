using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Infrastructure.Account.Configurations
{
	public class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.ToTable("Users", "Account");

			builder.HasKey(u => u.Id);

			builder.Property(u => u.FullName).IsRequired().HasMaxLength(150);
			builder.Property(u => u.Username).IsRequired().HasMaxLength(100);
			builder.Property(u => u.EmployeeCode).IsRequired().HasMaxLength(50);
			builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(256);
			builder.Property(u => u.Position).HasMaxLength(100);

			// Value Objects
			builder.OwnsOne(u => u.Email, email =>
			{
				email.Property(e => e.Value).HasColumnName("Email").HasMaxLength(255);
			});

			builder.OwnsOne(u => u.Phone, phone =>
			{
				phone.Property(p => p.Value).HasColumnName("Phone").HasMaxLength(30);
			});

			// Department (many-to-one)
			builder.HasOne(u => u.Department)
				.WithMany(d => d.Users)
				.HasForeignKey(u => u.DepartmentId)
				.OnDelete(DeleteBehavior.Restrict);

			// Manager (self-reference)
			builder.HasOne(u => u.Manager)
				.WithMany()
				.HasForeignKey(u => u.ManagerId)
				.OnDelete(DeleteBehavior.Restrict);

			// AvatarFile (StoredFile)
			builder.HasOne(u => u.AvatarFile)
				.WithMany()
				.HasForeignKey(u => u.AvatarFileId)
				.IsRequired(false)
				.OnDelete(DeleteBehavior.SetNull);

			// Field-based access for private collections (DDD)
			builder.HasMany(u => u.UserRoles)
				.WithOne(ur => ur.User)
				.HasForeignKey(ur => ur.UserId)
				.OnDelete(DeleteBehavior.Cascade);
			builder.Navigation(u => u.UserRoles).UsePropertyAccessMode(PropertyAccessMode.Field);

			// UserGroup
			builder.HasMany(u => u.UserGroups)
				.WithOne(ug => ug.User)
				.HasForeignKey(ug => ug.UserId)
				.OnDelete(DeleteBehavior.Cascade);
			builder.Navigation(u => u.UserGroups).UsePropertyAccessMode(PropertyAccessMode.Field);

			// ManagerAssignments
			builder.HasMany(u => u.ManagerAssignments)
				.WithOne(a => a.User)
				.HasForeignKey(a => a.UserId)
				.OnDelete(DeleteBehavior.Restrict);
			builder.Navigation(u => u.ManagerAssignments).UsePropertyAccessMode(PropertyAccessMode.Field);

			builder.HasMany(u => u.DirectReportsAssignments)
				.WithOne(a => a.Manager)
				.HasForeignKey(a => a.ManagerId)
				.OnDelete(DeleteBehavior.Restrict);
			builder.Navigation(u => u.DirectReportsAssignments).UsePropertyAccessMode(PropertyAccessMode.Field);

			builder.HasMany(u => u.BankAccounts)
				.WithOne(b => b.User)
				.HasForeignKey(b => b.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Navigation(u => u.BankAccounts)
				.UsePropertyAccessMode(PropertyAccessMode.Field);

			// Indexes
			builder.HasIndex(u => u.Username).IsUnique();
			builder.HasIndex(u => u.EmployeeCode).IsUnique();
		}
	}
}
