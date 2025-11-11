using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Linq.Expressions;
using ThaiTuanERP2025.Application.Common.Events;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Authentication.Entities;
using ThaiTuanERP2025.Domain.Common.Entities;
using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Files.Entities;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Infrastructure.Persistence
{
	public class ThaiTuanERP2025DbContext : DbContext
	{
		private readonly ICurrentUserService _currentUserService;
		private readonly IDomainEventDispatcher? _dispatcher;
		private bool _suppressDomainEvents = false;

		public ThaiTuanERP2025DbContext(
			DbContextOptions<ThaiTuanERP2025DbContext> options,
			ICurrentUserService currentUserService,
			IDomainEventDispatcher? dispatcher = null
		) : base(options) {
			_currentUserService = currentUserService;
			_dispatcher = dispatcher;
		}

		#region DbSet
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
		public DbSet<Follower> Followers => Set<Follower>();
		public DbSet<Role> Roles => Set<Role>();
		public DbSet<Permission> Permissions => Set<Permission>();
		public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
		public DbSet<UserRole> UserRoles => Set<UserRole>();
		public DbSet<UserNotification> UserNotifications => Set<UserNotification>();
		public DbSet<UserReminder> UserReminders => Set<UserReminder>();

		// Authentication
		public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
		#endregion

		#region Model Config
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			try
			{
				modelBuilder.ApplyConfigurationsFromAssembly(typeof(ThaiTuanERP2025DbContext).Assembly);
				ApplySoftDeleteFilters(modelBuilder);
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
		#endregion

		#region SaveChanges Overrides
		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			var now = DateTime.UtcNow;

			Guid? userId = null;
			if (_currentUserService?.UserId is Guid uid && uid != Guid.Empty)
				userId = uid; // chỉ khi có user hợp lệ

			foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
			{
				switch (entry.State)
				{
					case EntityState.Added:
						entry.Entity.CreatedDate = now;
						entry.Entity.CreatedByUserId = userId; // null nếu không có user
						break;

					case EntityState.Modified:
						entry.Entity.ModifiedDate = now;
						entry.Entity.ModifiedByUserId = userId; // null nếu không có user
						break;

						// nếu bạn có soft-delete chuyển state -> Modified, có thể set Deleted* ở nơi khác
				}
			}

			try
			{
				var result = await base.SaveChangesAsync(cancellationToken);

				// ✅ Chỉ dispatch domain events khi SaveChanges thành công
				if (!_suppressDomainEvents && _dispatcher is not null)
				{
					var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
					    .Select(e => e.Entity)
					    .Where(e => e.DomainEvents.Any())
					    .ToArray();

					var allEvents = entitiesWithEvents.SelectMany(e => e.DomainEvents).ToArray();

					if (allEvents.Length > 0)
						await _dispatcher.DispatchAsync(allEvents, cancellationToken);

					foreach (var entity in entitiesWithEvents)
						entity.ClearDomainEvents();
				}

				return result;
			}
			catch (DbUpdateConcurrencyException ex)
			{
				var details = ex.Entries.Select(e =>
				{
					var concProps = e.Properties.Where(p => p.Metadata.IsConcurrencyToken)
						.Select(p => $"{p.Metadata.Name}: orig={p.OriginalValue} / curr={p.CurrentValue}");

					var pk = string.Join(",", e.Properties.Where(p => p.Metadata.IsPrimaryKey()).Select(p => $"{p.Metadata.Name}={p.CurrentValue}"));
					return $"{e.Entity.GetType().Name} PK[{pk}] | Concurrency[{string.Join("; ", concProps)}]";
				});

				throw new DbUpdateConcurrencyException("Concurrency conflict: " + string.Join(" || ", details), ex);
			}
		}

		/// Lưu mà không phát domain events (dùng trong handler hoặc service hạ tầng để tránh loop).
		public async Task<int> SaveChangesWithoutDispatchAsync(CancellationToken cancellationToken = default)
		{
			var prev = _suppressDomainEvents;
			_suppressDomainEvents = true;

			try
			{
				return await SaveChangesAsync(cancellationToken);
			}
			finally
			{
				_suppressDomainEvents = prev; // khôi phục trạng thái cũ
			}
		}

		/// Cho phép tạm thời bật/tắt domain events bằng code.
		public void SuppressDomainEvents(bool suppress = true)
		{
			_suppressDomainEvents = suppress;
		}
		#endregion

		#region Soft Delete Filter
		private static void ApplySoftDeleteFilters(ModelBuilder modelBuilder)
		{
			var entityClrTypes = modelBuilder.Model
			    .GetEntityTypes()
			    .Select(et => et.ClrType)
			    .Where(t => t != null && typeof(AuditableEntity).IsAssignableFrom(t))
			    .Distinct()!; // phòng trùng nếu có shared types

			foreach (var clrType in entityClrTypes)
			{
				var param = Expression.Parameter(clrType, "e");
				var prop = Expression.Property(param, nameof(AuditableEntity.IsDeleted));
				var body = Expression.Equal(prop, Expression.Constant(false));

				var lambdaType = typeof(Func<,>).MakeGenericType(clrType, typeof(bool));
				var lambda = Expression.Lambda(lambdaType, body, param);

				modelBuilder.Entity(clrType).HasQueryFilter(lambda);
			}
		}
		#endregion
	}
}
