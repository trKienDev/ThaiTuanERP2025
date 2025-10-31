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

			// ===== Value Object: Email =====
			builder.OwnsOne(u => u.Email, email =>
			{
				email.Property(e => e.Value)
					.HasColumnName("Email")
					.HasMaxLength(255);
			});

			// ===== Value Object: Phone =====
			builder.OwnsOne(u => u.Phone, phone =>
			{
				phone.Property(p => p.Value)
					.HasColumnName("Phone")
					.HasMaxLength(30);
			});

			// ===== Department =====
			builder.HasOne(u => u.Department)
				.WithMany(d => d.Users)
				.HasForeignKey(u => u.DepartmentId)
				.OnDelete(DeleteBehavior.Restrict);

			// ===== Manager =====
			builder.HasOne(u => u.Manager)
				.WithMany()
				.HasForeignKey(u => u.ManagerId)
				.OnDelete(DeleteBehavior.Restrict);

			// ===== AvatarFile =====
			builder.HasOne<StoredFile>()
				.WithMany()
				.HasForeignKey(u => u.AvatarFileId)
				.IsRequired(false)
				.OnDelete(DeleteBehavior.SetNull);

			// ===== Indexes =====
			builder.HasIndex(u => u.Username).IsUnique();
			builder.HasIndex(u => u.EmployeeCode).IsUnique();

			// ===== Field-based access (SAFE VERSION) =====
			SetFieldAccessModeSafe(builder, nameof(User.UserRoles));
			SetFieldAccessModeSafe(builder, nameof(User.UserGroups));
			SetFieldAccessModeSafe(builder, nameof(User.ManagerAssignments));
			SetFieldAccessModeSafe(builder, nameof(User.DirectReportsAssignments));
			SetFieldAccessModeSafe(builder, nameof(User.BankAccounts));
		}

		private static void SetFieldAccessModeSafe<TEntity>(EntityTypeBuilder<TEntity> builder, string navigationName)
			where TEntity : class
		{
			var navigation = builder.Metadata.FindNavigation(navigationName);
			if (navigation != null)
				navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}

}
