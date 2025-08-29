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
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Files.Entities;
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
		public DbSet<NumberSeries> NumberSeries => Set<NumberSeries>();
		public DbSet<LedgerAccountType> LedgerAccountTypes => Set<LedgerAccountType>();
		public DbSet<LedgerAccount> LedgerAccounts => Set<LedgerAccount>();
		public DbSet<Tax> Taxes => Set<Tax>();
		public DbSet<WithholdingTaxType> WithholdingTaxTypes => Set<WithholdingTaxType>();
		public DbSet<CashoutGroup> CashOutGroups => Set<CashoutGroup>();
		public DbSet<CashoutCode> CashOutCodes => Set<CashoutCode>();
		public DbSet<StoredFile> StoredFiles => Set<StoredFile>();
		public DbSet<BankAccount> BankAccounts => Set<BankAccount>();
		public DbSet<Supplier> Suppliers => Set<Supplier>();
		public DbSet<Invoice> Invoices => Set<Invoice>();
		public DbSet<InvoiceLine> InvoiceLines => Set<InvoiceLine>();
		public DbSet<InvoiceFile> InvoiceFiles => Set<InvoiceFile>();
		public DbSet<InvoiceFollwer> InvoiceFollwers => Set<InvoiceFollwer>();	
		public DbSet<ApprovalFlowDefinition> ApprovalFlowDefinitions => Set<ApprovalFlowDefinition>();	
		public DbSet<ApprovalStepDefinition> ApprovalStepDefinitions => Set<ApprovalStepDefinition>();
		public DbSet<ApprovalFlowInstance> ApprovalFlowInstance => Set<ApprovalFlowInstance>();
		public DbSet<ApprovalStepInstance> ApprovalStepInstances => Set<ApprovalStepInstance>();
		public DbSet<ApprovalAction> ApprovalActions => Set<ApprovalAction>();


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(ThaiTuanERP2025DbContext).Assembly);
			ApplySoftDeleteFilters(modelBuilder);
			ConfigureNumberSeries(modelBuilder);
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

		private static void ApplySoftDeleteFilters(ModelBuilder modelBuilder)
		{
			foreach (var entityType in modelBuilder.Model.GetEntityTypes())
			{
				var clrType = entityType.ClrType;

				// Bỏ qua các type không phải entity thực (owned/skip navigations etc.)
				if (clrType == null || !typeof(AuditableEntity).IsAssignableFrom(clrType))
					continue;

				var parameter = Expression.Parameter(clrType, "e");
				var prop = Expression.Property(parameter, nameof(AuditableEntity.IsDeleted));
				var body = Expression.Not(prop); // !e.IsDeleted
				var lambda = Expression.Lambda(body, parameter);

				modelBuilder.Entity(clrType).HasQueryFilter(lambda);
			}
		}

	}
}
