using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Infrastructure.Persistence
{
	public class ThaiTuanERP2025DbContext : DbContext
	{
		private readonly ICurrentUserService _currentUserService;
		public ThaiTuanERP2025DbContext(
			DbContextOptions<ThaiTuanERP2025DbContext> options,
			ICurrentUserService currentUserService	
		) : base(options) {
			_currentUserService = currentUserService;
		}

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
			ConfigureFinance(modelBuilder);
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
				builder.HasIndex(g => g.Code).IsUnique();
				builder.HasKey(g => g.Id);
				builder.Property(g => g.Name).IsRequired().HasMaxLength(100);
				builder.Property(g => g.Code).IsRequired().HasMaxLength(255);
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

		private void ConfigureFinance(ModelBuilder modelBuilder) {
			modelBuilder.Entity<BudgetGroup>(builder =>
			{
				builder.HasKey(e => e.Id);
				builder.HasIndex(e => e.Code).IsUnique();
				builder.Property(e => e.Code).IsRequired().HasMaxLength(50);
				builder.Property(e => e.Name).IsRequired().HasMaxLength(255);
			});

			modelBuilder.Entity<BudgetCode>(builder =>
			{
				builder.HasKey(e => e.Id);
				builder.HasIndex(e => e.Code).IsUnique();
				builder.Property(e => e.Code).IsRequired().HasMaxLength(50);
				builder.Property(e => e.Name).IsRequired().HasMaxLength(255);
				builder.HasOne(e => e.BudgetGroup)
					.WithMany(g => g.BudgetCodes)
					.HasForeignKey(e => e.BudgetGroupId)
					.OnDelete(DeleteBehavior.Restrict);
			});

			modelBuilder.Entity<BudgetPeriod>(builder =>
			{
				builder.HasKey(e => e.Id);
				builder.HasIndex(e => new { e.Year, e.Month }).IsUnique();
				builder.Property(e => e.Year).IsRequired();
				builder.Property(e => e.Month).IsRequired();
			});

			modelBuilder.Entity<BudgetPlan>(builder =>
			{
				builder.HasKey(e => e.Id);
				builder.Property(e => e.Amount).HasColumnType("decimal(18, 2)").IsRequired();
				builder.Property(e => e.Status).HasMaxLength(50).IsRequired();
				builder.HasIndex(e => new { e.DepartmentId, e.BudgetCodeId, e.BudgetPeriodId }).IsUnique();
				builder.HasOne(e => e.BudgetCode)
					.WithMany(c => c.BudgetPlans)
					.HasForeignKey(e => e.BudgetCodeId)
					.OnDelete(DeleteBehavior.Restrict);
				builder.HasOne(e => e.BudgetPeriod)
					.WithMany(p => p.BudgetPlans)
					.HasForeignKey(e => e.BudgetPeriodId)
					.OnDelete(DeleteBehavior.Restrict);
			});

			modelBuilder.Entity<BankAccount>(builder =>
			{
				builder.HasKey(e => e.Id);
				builder.Property(e => e.AccountNumber).IsRequired().HasMaxLength(50);
				builder.Property(e => e.BankName).IsRequired().HasMaxLength(100);
				builder.Property(e => e.AccountHolder).IsRequired().HasMaxLength(100);
				builder.Property(e => e.EmployeeCode).HasMaxLength(50);
				builder.Property(e => e.Note).HasMaxLength(500);
				builder.Property(e => e.CustomerName).HasMaxLength(250);
				builder.HasOne<Department>()
					.WithMany()
					.HasForeignKey(e => e.DepartmentId)
					.OnDelete(DeleteBehavior.Restrict);
			});
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			var entries = ChangeTracker.Entries<AuditableEntity>();
			var currentUserId = _currentUserService?.UserId ?? Guid.Empty;
			foreach (var entry in entries) {
				switch (entry.State)
				{
					case EntityState.Added:
						entry.Entity.CreatedByUserId = currentUserId;
						entry.Entity.CreatedDate = DateTime.UtcNow;
						break;
					case EntityState.Modified:
						entry.Entity.DateModified = DateTime.UtcNow;
						entry.Entity.ModifiedByUserId = currentUserId;
						break;
					case EntityState.Deleted:
						// Thay vì xóa thực tế, đánh dấu là đã xóa
						entry.Entity.IsDeleted = true;
						entry.Entity.DeletedDate = DateTime.UtcNow;
						entry.Entity.DeletedByUserId = currentUserId;
						entry.State = EntityState.Modified; // Chuyển sang trạng thái Modified để lưu lại thông tin xóa
						break;
				}
			}
			return base.SaveChangesAsync(cancellationToken);
		}

		private void ApplyGlobalFilters(ModelBuilder modelBuilder)
		{
			foreach(var entityType in modelBuilder.Model.GetEntityTypes())
			{
				if (typeof(AuditableEntity).IsAssignableFrom(entityType.ClrType))
				{
					var method = typeof(ThaiTuanERP2025DbContext)
						.GetMethod(nameof(SetSoftDeleteFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
						? .MakeGenericMethod(entityType.ClrType);

					method?.Invoke(null, new object[] { modelBuilder });
				}
			}
		}

		private static void SetSoftDeleteFilter<TEntity>(ModelBuilder builder) where TEntity : AuditableEntity
		{
			builder.Entity<TEntity>().HasQueryFilter(e => !e.IsDeleted);
		}
	}
}
