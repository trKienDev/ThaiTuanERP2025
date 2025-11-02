using ThaiTuanERP2025.Domain.Account.Repositories;
using ThaiTuanERP2025.Domain.Alerts.Repositories;
using ThaiTuanERP2025.Domain.Expense.Repositories;
using ThaiTuanERP2025.Domain.Files.Repositories;
using ThaiTuanERP2025.Domain.Finance.Repositories;
using ThaiTuanERP2025.Domain.Followers.Repositories;

namespace ThaiTuanERP2025.Application.Common.Interfaces
{
	public interface IUnitOfWork
	{
		// Define the DbContext type

		// Account
		IUserWriteRepository Users { get; }
		IUserManagerAssignmentRepository UserManagerAssignments { get; }
		IDepartmentWriteRepository Departments { get; }
		IGroupRepository Groups { get; }
		IUserGroupRepository UserGroups { get; }

		// Finance
		IBudgetCodeRepository BudgetCodes { get; }
		IBudgetGroupRepository BudgetGroups { get; }
		IBudgetPeriodRepository BudgetPeriods { get; }
		IBudgetPlanRepository BudgetPlans { get; }

		ILedgerAccountRepository LedgerAccounts { get; }
		ILedgerAccountTypeRepository LedgerAccountTypes { get; }
		ICashoutCodeRepository CashoutCodes { get; }
		ICashoutGroupRepository CashoutGroups { get; }

		// Files
		IStoredFilesRepository StoredFiles { get; }

		// Expense
		IInvoiceRepository Invoices { get; }
		IInvoiceFileRepository InvoiceFiles { get; }
		ISupplierRepository Suppliers { get; }
		IBankAccountRepository BankAccounts { get; }
		IOutgoingBankAccountRepository OutgoingBankAccounts { get; }
		IOutgoingPaymentRepository OutgoingPayments { get; }

		// Workflow
		IExpenseStepTemplateRepository ApprovalStepTemplates { get; }
		IExpenseWorkflowTemplateRepository ApprovalWorkflowTemplates { get; }
		IExpenseWorkflowInstanceRepository ApprovalWorkflowInstances { get; }
		IExpenseStepInstanceRepository ApprovalStepInstances { get; }

		// Expense Payment
		IExpensePaymentRepository ExpensePayments { get; }
		IExpensePaymentCommentRepository ExpensePaymentComments { get; }
		IExpensePaymentCommentAttachmentRepository ExpensePaymentCommentAttachments { get; }
		IExpensePaymentCommentTagRepository ExpensePaymentCommentTags { get; }

		// Notification
		INotificationWriteRepository Notifications { get; }
		ITaskReminderWriteRepository TaskReminders { get; }

		// Follow
		IFollowerRepository Followers { get; }

		// RBAC
		IRoleWriteRepository Roles { get; }
		IRolePermissionRepository RolePermissions { get; }
		IPermissionRepository Permissions { get; }
		IUserRoleRepository UserRoles { get; }

		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}
