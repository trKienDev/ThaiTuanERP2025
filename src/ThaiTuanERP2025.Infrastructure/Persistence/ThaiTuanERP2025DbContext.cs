using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Infrastructure.Persistence
{
	public class ThaiTuanERP2025DbContext : DbContext
	{
		public ThaiTuanERP2025DbContext(DbContextOptions<ThaiTuanERP2025DbContext> options) : base(options) { }

		// DbSet
		public DbSet<User> Users => Set<User>();
		public DbSet<Department> Departments => Set<Department>();
		public DbSet<Group> Groups => Set<Group>();
		public DbSet<UserGroup> UserGroups => Set<UserGroup>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			ConfigureUser(modelBuilder);
			ConfigureDepartment(modelBuilder);
			ConfigureGroup(modelBuilder);
			ConfigureUserGroup(modelBuilder);
		}

		private void ConfigureUser(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>(builder =>
			{
				builder.HasKey(u => u.Id);

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
					.OnDelete(DeleteBehavior.Restrict);
				builder.HasOne(u => u.Manager).WithMany().HasForeignKey(u => u.ManagerId).OnDelete(DeleteBehavior.Restrict);
			});
		}

		private void ConfigureDepartment(ModelBuilder modelBuilder) {
			modelBuilder.Entity<Department>(builder =>
			{
				builder.HasKey(g => g.Id);
				builder.Property(g => g.Name).IsRequired().HasMaxLength(100);
				builder.Property(g => g.Code).HasMaxLength(255);
			});
		}
		private void ConfigureGroup(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Group>(builder => {
				builder.HasKey(g => g.Id);
				builder.Property(g => g.Name).IsRequired().HasMaxLength(100);
				builder.Property(g => g.Description).HasMaxLength(255);
			});
		}
		private void ConfigureUserGroup(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<UserGroup>(builder => { 
				builder.HasKey(ug => new { ug.UserId, ug.GroupId });
				builder.HasOne(ug => ug.User).WithMany(u => u.UserGroups).HasForeignKey(ug => ug.UserId);
				builder.HasOne(ug => ug.Group).WithMany(g => g.UserGroups).HasForeignKey(ug => ug.GroupId);
			});
		}
	}
}
