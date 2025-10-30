using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Files.Entities;

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
			builder.HasOne<StoredFile>()
				.WithMany()
				.HasForeignKey(u => u.AvatarFileId)
				.IsRequired(false)
				.OnDelete(DeleteBehavior.SetNull);

			// Field-based access for private collections (DDD)
			builder.Metadata.FindNavigation(nameof(User.UserRoles))!
				.SetPropertyAccessMode(PropertyAccessMode.Field);
			
			builder.Metadata.FindNavigation(nameof(User.UserGroups))!
				.SetPropertyAccessMode(PropertyAccessMode.Field);
			
			builder.Metadata.FindNavigation(nameof(User.ManagerAssignments))!
				.SetPropertyAccessMode(PropertyAccessMode.Field);
			
			builder.Metadata.FindNavigation(nameof(User.DirectReportsAssignments))!
				.SetPropertyAccessMode(PropertyAccessMode.Field);
			
			builder.Metadata.FindNavigation(nameof(User.BankAccounts))!
				.SetPropertyAccessMode(PropertyAccessMode.Field);

			// Indexes
			builder.HasIndex(u => u.Username).IsUnique();
			builder.HasIndex(u => u.EmployeeCode).IsUnique();
			builder.HasIndex("Email");
		}
	}
}
