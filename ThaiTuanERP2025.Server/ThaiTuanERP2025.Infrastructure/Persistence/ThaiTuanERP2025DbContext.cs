using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Linq.Expressions;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Alerts.Entities;
using ThaiTuanERP2025.Domain.Authentication.Entities;
using ThaiTuanERP2025.Domain.Common.Entities;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Files.Entities;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Domain.Followers.Entities;
using ThaiTuanERP2025.Infrastructure.Common;

namespace ThaiTuanERP2025.Infrastructure.Persistence
{
	public class ThaiTuanERP2025DbContext : DbContext
	{
		private readonly ICurrentUserService _currentUserService;
		private readonly DomainEventDispatcher? _dispatcher;

		public ThaiTuanERP2025DbContext(
			DbContextOptions<ThaiTuanERP2025DbContext> options,
			ICurrentUserService currentUserService,
			DomainEventDispatcher? dispatcher = null
		) : base(options) {
			_currentUserService = currentUserService;
			_dispatcher = dispatcher;
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
		public DbSet<CashoutGroup> CashOutGroups => Set<CashoutGroup>();
		public DbSet<CashoutCode> CashOutCodes => Set<CashoutCode>();
		public DbSet<StoredFile> StoredFiles => Set<StoredFile>();
		public DbSet<BankAccount> BankAccounts => Set<BankAccount>();
		public DbSet<OutgoingBankAccount> OutgoingBankAccounts => Set<OutgoingBankAccount>();
		public DbSet<Supplier> Suppliers => Set<Supplier>();
		public DbSet<Invoice> Invoices => Set<Invoice>();
		public DbSet<InvoiceFile> InvoiceFiles => Set<InvoiceFile>();
		public DbSet<BudgetCode> BudgetCodes => Set<BudgetCode>();
		public DbSet<BudgetGroup> BudgetGroups => Set<BudgetGroup>();
		public DbSet<BudgetPeriod> BudgetPeriods => Set<BudgetPeriod>();
		public DbSet<BudgetPlan> BudgetPlans => Set<BudgetPlan>();
		public DbSet<ExpenseWorkflowTemplate> ApprovalWorkflowTemplates => Set<ExpenseWorkflowTemplate>();
		public DbSet<ExpenseStepTemplate> ApproverStepTemplates => Set<ExpenseStepTemplate>();
		public DbSet<ExpenseWorkflowInstance> ApprovalWorkflowInstances => Set<ExpenseWorkflowInstance>();
		public DbSet<ExpenseStepInstance> ApprovalStepInstances => Set<ExpenseStepInstance>();
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

		// Authentication
		public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			try
			{
				modelBuilder.ApplyConfigurationsFromAssembly(typeof(ThaiTuanERP2025DbContext).Assembly);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Lỗi trong ApplyConfigurationsFromAssembly:");
				Console.WriteLine(ex);
				throw;
			}
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);

			// ⚠️ Bỏ qua cảnh báo "PendingModelChangesWarning"
			optionsBuilder.ConfigureWarnings(w =>
			    w.Ignore(RelationalEventId.PendingModelChangesWarning));
		}

		public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
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
			
			var result = await base.SaveChangesAsync(cancellationToken);

			if (_dispatcher is not null)
			{
				var entitiesWithEvents = ChangeTracker.Entries<AuditableEntity>()
					.Select(e => e.Entity)
					.Where(e => e.DomainEvents.Any())
					.ToArray();

				// Flatten tất cả domain events
				var allEvents = entitiesWithEvents
					.SelectMany(e => e.DomainEvents)
					.ToArray();

				await _dispatcher.DispatchAsync(allEvents, cancellationToken);

				// Optionally: clear domain events sau khi publish
				foreach (var entity in entitiesWithEvents)
					entity.ClearDomainEvents();
			}

			return result;
		}

		private static void ApplySoftDeleteFilters(ModelBuilder modelBuilder)
		{
			foreach (var entityType in modelBuilder.Model.GetEntityTypes())
			{
				var clrType = entityType.ClrType;
				if (clrType == null || !typeof(AuditableEntity).IsAssignableFrom(clrType))
					continue;

				// Kiểm tra có property IsDeleted hay không
				var propInfo = clrType.GetProperty(nameof(AuditableEntity.IsDeleted));
				if (propInfo == null)
					continue;

				var parameter = Expression.Parameter(clrType, "e");
				var prop = Expression.Property(parameter, propInfo);
				var body = Expression.Not(prop);
				var lambda = Expression.Lambda(body, parameter);

				modelBuilder.Entity(clrType).HasQueryFilter(lambda);
			}
		}
	}
}
