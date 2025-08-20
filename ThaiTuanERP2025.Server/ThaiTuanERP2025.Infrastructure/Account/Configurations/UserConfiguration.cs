using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Infrastructure.Account.Configurations
{
	public class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.ToTable("Users", "Core");
			builder.HasKey(x => x.Id);

			builder.Property(u => u.FullName).IsRequired().HasMaxLength(255);
			builder.Property(u => u.Username).IsRequired().HasMaxLength(100);
			builder.Property(u => u.EmployeeCode).IsRequired().HasMaxLength(50);
			builder.Property(u => u.PasswordHash).IsRequired();
			builder.Property(u => u.AvatarUrl).HasMaxLength(500);
			builder.Property(u => u.Role).HasMaxLength(100); // nhân viên, quản lý, giám đốc, PGĐ, TGĐ
			builder.Property(u => u.Position).HasMaxLength(100); // nhân viên IT

			// Owned types
			builder.OwnsOne(u => u.Email, email =>
			{
				email.Property(e => e.Value).HasColumnName("Email").HasMaxLength(255);
			});
			builder.OwnsOne(u => u.Phone, phone =>
			{
				phone.Property(p => p.Value).HasColumnName("Phone").HasMaxLength(20);
			});

			// Relationships
			builder.HasOne(u => u.Department)
				.WithMany(d => d.Users)
				.HasForeignKey(u => u.DepartmentId)
				.IsRequired(false)
				.OnDelete(DeleteBehavior.Restrict);
			builder.HasOne(u => u.Manager).WithMany().HasForeignKey(u => u.ManagerId).OnDelete(DeleteBehavior.Restrict);
		}
	}
}
