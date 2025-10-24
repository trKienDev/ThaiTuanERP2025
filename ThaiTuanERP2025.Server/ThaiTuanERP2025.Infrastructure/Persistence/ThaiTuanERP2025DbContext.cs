using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Linq.Expressions;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Files.Entities;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Domain.Followers.Entities;
using ThaiTuanERP2025.Domain.Notifications;
using ThaiTuanERP2025.Infrastructure.Persistence.Seeds;

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
		public DbSet<UserManagerAssignment> UserManagerAssignments => Set<UserManagerAssignment>();
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
		public DbSet<OutgoingBankAccount> OutgoingBankAccounts => Set<OutgoingBankAccount>();
		public DbSet<Supplier> Suppliers => Set<Supplier>();
		public DbSet<Invoice> Invoices => Set<Invoice>();
		public DbSet<InvoiceLine> InvoiceLines => Set<InvoiceLine>();
		public DbSet<InvoiceFile> InvoiceFiles => Set<InvoiceFile>();
		public DbSet<InvoiceFollwer> InvoiceFollwers => Set<InvoiceFollwer>();	
		public DbSet<BudgetCode> BudgetCodes => Set<BudgetCode>();
		public DbSet<BudgetGroup> BudgetGroups => Set<BudgetGroup>();
		public DbSet<BudgetPeriod> BudgetPeriods => Set<BudgetPeriod>();
		public DbSet<BudgetPlan> BudgetPlans => Set<BudgetPlan>();
		public DbSet<ApprovalWorkflowTemplate> ApprovalWorkflowTemplates => Set<ApprovalWorkflowTemplate>();
		public DbSet<ApprovalStepTemplate> ApproverStepTemplates => Set<ApprovalStepTemplate>();
		public DbSet<ApprovalWorkflowInstance> ApprovalWorkflowInstances => Set<ApprovalWorkflowInstance>();
		public DbSet<ApprovalStepInstance> ApprovalStepInstances => Set<ApprovalStepInstance>();
		public DbSet<ExpensePayment> ExpensePayments => Set<ExpensePayment>();	
		public DbSet<ExpensePaymentItem> ExpensePaymentItems => Set<ExpensePaymentItem>();
		public DbSet<ExpensePaymentAttachment> ExpensePaymentAttachments => Set<ExpensePaymentAttachment>();
		public DbSet<ExpensePaymentComment> ExpensePaymentComments => Set<ExpensePaymentComment>();
		public DbSet<ExpensePaymentCommentAttachment> ExpensePaymentCommentAttachments => Set<ExpensePaymentCommentAttachment>();
		public DbSet<ExpensePaymentCommentTag> expensePaymentCommentTags => Set<ExpensePaymentCommentTag>();
		public DbSet<OutgoingPayment> outgoingPayments => Set<OutgoingPayment>();
		public DbSet<AppNotification> AppNotification => Set<AppNotification>();
		public DbSet<TaskReminder> TaskReminders => Set<TaskReminder>();
		public DbSet<Follower> Followers => Set<Follower>();
		public DbSet<Role> Roles => Set<Role>();
		public DbSet<Permission> Permissions => Set<Permission>();
		public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
		public DbSet<UserRole> UserRoles => Set<UserRole>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.SeedRolesAndPermissions();
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

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);

			// ⚠️ Bỏ qua cảnh báo "PendingModelChangesWarning"
			optionsBuilder.ConfigureWarnings(w =>
			    w.Ignore(RelationalEventId.PendingModelChangesWarning));
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
