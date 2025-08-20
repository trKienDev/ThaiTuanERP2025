using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Domain.Partner.Entities;

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
		public DbSet<PartnerBankAccount> PartnerBankAccounts => Set<PartnerBankAccount>();
		public DbSet<NumberSeries> NumberSeries => Set<NumberSeries>();
		public DbSet<LedgerAccountType> LedgerAccountTypes => Set<LedgerAccountType>();
		public DbSet<LedgerAccount> LedgerAccounts => Set<LedgerAccount>();
		public DbSet<Tax> Taxes => Set<Tax>();
		public DbSet<CashOutGroup> CashOutGroups => Set<CashOutGroup>();
		public DbSet<CashOutCode> CashOutCodes => Set<CashOutCode>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(ThaiTuanERP2025DbContext).Assembly);
			ConfigureFinance(modelBuilder);
			ConfigurePartner(modelBuilder);
			ApplyGlobalFilters(modelBuilder);
			ConfigureNumberSeries(modelBuilder);
		}

		private void ConfigureFinance(ModelBuilder modelBuilder) {

			modelBuilder.Entity<BankAccount>(builder =>
			{
				builder.HasKey(e => e.Id);
				builder.Property(e => e.AccountNumber).IsRequired().HasMaxLength(50);
				builder.Property(e => e.BankName).IsRequired().HasMaxLength(100);
				builder.Property(e => e.AccountHolder).IsRequired().HasMaxLength(100);
				builder.Property(e => e.EmployeeCode).HasMaxLength(50);
				builder.Property(e => e.Note).HasMaxLength(500);
				builder.Property(e => e.OwnerName).HasMaxLength(250);

				builder.HasOne(e => e.CreatedByUser)
					.WithMany().HasForeignKey(e => e.CreatedByUserId)
					.OnDelete(DeleteBehavior.Restrict);
				builder.HasOne(e => e.ModifiedByUser)
					.WithMany().HasForeignKey(e => e.ModifiedByUserId)
					.OnDelete(DeleteBehavior.Restrict);
				builder.HasOne(e => e.DeletedByUser)
					.WithMany().HasForeignKey(e => e.DeletedByUserId)
					.OnDelete(DeleteBehavior.Restrict);
			});
		}

		private void ConfigurePartner(ModelBuilder modelBuilder) {
			modelBuilder.Entity<PartnerBankAccount>(b =>
			{
				b.ToTable("PartnerBankAccounts");
				b.HasKey(x => x.Id);

				b.HasIndex(x => x.SupplierId).IsUnique(); // ép 1–1

				b.Property(x => x.AccountNumber).IsRequired().HasMaxLength(50);
				b.Property(x => x.BankName).IsRequired().HasMaxLength(150);
				b.Property(x => x.AccountHolder).HasMaxLength(150);
				b.Property(x => x.SwiftCode).HasMaxLength(11); // BIC 8 hoặc 11 ký tự
				b.Property(x => x.Branch).HasMaxLength(150);
				b.Property(x => x.Note).HasMaxLength(500);

				// Quan hệ 1–1 với Supplier
				b.HasOne(p => p.Supplier)
					.WithOne(s => s.BankAccount) // hoặc .WithOne(s => s.BankAccount) nếu bạn thêm navigation ở Supplier
					.HasForeignKey<PartnerBankAccount>(p => p.SupplierId)
					.OnDelete(DeleteBehavior.Restrict);

				// Audit FK nếu bạn đang áp dụng tương tự các entity khác:
				b.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(e => e.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
				b.HasOne(e => e.ModifiedByUser).WithMany().HasForeignKey(e => e.ModifiedByUserId).OnDelete(DeleteBehavior.Restrict);
				b.HasOne(e => e.DeletedByUser).WithMany().HasForeignKey(e => e.DeletedByUserId).OnDelete(DeleteBehavior.Restrict);
			});
		}
		
		private void ConfigureNumberSeries(ModelBuilder modelBuilder) {
			modelBuilder.Entity<NumberSeries>(b =>
			{
				b.ToTable("NumberSeries");
				b.HasKey(x => x.Id);

				b.HasIndex(x => x.Key).IsUnique();
				b.Property(x => x.Key).IsRequired().HasMaxLength(100);

				b.Property(x => x.Prefix).IsRequired().HasMaxLength(20);
				b.Property(x => x.PadLength).IsRequired().HasDefaultValue(6); // độ dài padding, mặc định 6 ký tự
				b.Property(x => x.NextNumber).IsRequired();

				b.Property(x => x.RowVersion).IsRowVersion(); // concurrency token
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

		/// <summary>
		/// Áp dụng bộ lọc toàn cục cho các entity kế thừa AuditableEntity
		/// !IsDeleted cho tất cả entity kế thừa AuditableEntity.
		/// Nếu entity có navigation tới cha mà cha cũng kế thừa AuditableEntity ⇒ tự động thêm điều kiện !Parent.IsDeleted.
		/// logic: ẩn con khi cha bị ẩn
		/// </summary>
		/// <param name="modelBuilder"></param>
		private void ApplyGlobalFilters(ModelBuilder modelBuilder)
		{
			foreach(var entityType in modelBuilder.Model.GetEntityTypes())
			{
				var clrType = entityType.ClrType;

				// Bỏ qua entity không kế thừa AuditableEntity
				if(!typeof(AuditableEntity).IsAssignableFrom(clrType))
					continue;
				
				var parameter = Expression.Parameter(clrType, "e");

				// Điều kiện chính: !e.IsDeleted
				var isDeletedProperty = Expression.Property(parameter, nameof(AuditableEntity.IsDeleted));
				var notDeleted = Expression.Not(isDeletedProperty);

				Expression finalFilter = notDeleted;

				// Duyệt qua navigation tới cha
				foreach (var navigation in entityType.GetNavigations())
				{
					var targetClrType = navigation.TargetEntityType.ClrType;
					if (typeof(AuditableEntity).IsAssignableFrom(targetClrType) && navigation.IsOnDependent)
					{
						// e.Parent != null && !e.Parent.IsDeleted	
						var navProperty = Expression.Property(parameter, navigation.Name);
						var parentNotNull = Expression.NotEqual(navProperty, Expression.Constant(null, targetClrType));
						var parentIsDeletedProperty = Expression.Property(navProperty, nameof(AuditableEntity.IsDeleted));
						var parentNotDeleted = Expression.Not(parentIsDeletedProperty);
						var parentCondition = Expression.AndAlso(parentNotNull, parentNotDeleted);

						finalFilter = Expression.AndAlso(finalFilter, parentCondition);
					}
				}

				var lamba = Expression.Lambda(finalFilter, parameter);
				modelBuilder.Entity(clrType).HasQueryFilter(lamba);
			}
		}
	}
}
