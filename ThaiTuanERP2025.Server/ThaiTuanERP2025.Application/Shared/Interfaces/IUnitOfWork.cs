using ThaiTuanERP2025.Domain.Account.Repositories;
using ThaiTuanERP2025.Domain.Core.Repositories;
using ThaiTuanERP2025.Domain.Expense.Repositories;
using ThaiTuanERP2025.Domain.Files.Repositories;
using ThaiTuanERP2025.Domain.Finance.Repositories;
namespace ThaiTuanERP2025.Domain.Shared.Repositories
{
	public interface IUnitOfWork
	{
		// Define the DbContext type

		// Account
		 IUserWriteRepository Users { get; }
		IUserManagerAssignmentWriteRepository UserManagerAssignments { get; }
		 IDepartmentWriteRepository Departments { get; }
		IGroupRepository Groups { get; }
		IUserGroupRepository UserGroups { get; }

		// Finance
		IBudgetCodeWriteRepository BudgetCodes { get; }
		IBudgetGroupWriteRepository BudgetGroups { get; }
		IBudgetPeriodWriteRepository BudgetPeriods { get; }
		IBudgetPlanWriteRepository BudgetPlans { get; }
		IBudgetPlanDetailWriteRepository BudgetPlanDetails { get; }	
		IBudgetApproverWriteRepository BudgetApprovers { get; }
		IBudgetTransactionWriteRepository BudgetTransactions { get; }
		ILedgerAccountWriteRepository LedgerAccounts { get; }
		ILedgerAccountTypeWriteRepository LedgerAccountTypes { get; }
		ICashoutCodeWriteRepository CashoutCodes { get; }
		ICashoutGroupWriteRepository CashoutGroups { get; }

		// Files
		IStoredFilesRepository StoredFiles { get; }

		// Expense
		ISupplierWriteRepository Suppliers { get; }
		IOutgoingBankAccountRepository OutgoingBankAccounts { get; }
		IOutgoingPaymentRepository OutgoingPayments { get; }

		// Workflow
		IExpenseStepTemplateWriteRepository ExpenseStepTemplates { get; }
		IExpenseWorkflowTemplateWriteRepository ExpenseWorkflowTemplates { get; }
		IExpenseWorkflowInstanceWriteRepository ExpenseWorkflowInstances { get; }
		IExpenseStepInstanceRepository ExpenseStepInstances { get; }

		// Expense Payment
		IExpensePaymentWriteRepository ExpensePayments { get; }
		IExpensePaymentItemsWriteRepository ExpensePaymentItems { get; }
		IExpensePaymentCommentRepository ExpensePaymentComments { get; }
		IExpensePaymentCommentAttachmentRepository ExpensePaymentCommentAttachments { get; }
		IExpensePaymentCommentTagRepository ExpensePaymentCommentTags { get; }

		// Core
		IFollowerWriteRepository Followers { get; }
		IUserNotificationWriteRepository UserNotifications { get; }
		IUserReminderWriteRepository UserReminders { get; }
		IOutboxMessageWriteRepository OutboxMessages { get; }	

		// RBAC
		IRoleWriteRepository Roles { get; }
		IRolePermissionRepository RolePermissions { get; }
		IPermissionWriteRepository Permissions { get; }
		IUserRoleRepository UserRoles { get; }

		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
		Task<int> SaveChangesWithoutDispatchAsync(CancellationToken cancellationToken = default);
	}
}
