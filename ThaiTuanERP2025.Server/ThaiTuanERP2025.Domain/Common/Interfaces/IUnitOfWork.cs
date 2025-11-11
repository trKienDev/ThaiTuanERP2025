using ThaiTuanERP2025.Domain.Account.Repositories;
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
		IBudgetCodeWriteRepository BudgetCodes { get; }
		IBudgetGroupWriteRepository BudgetGroups { get; }
		IBudgetPeriodWriteRepository BudgetPeriods { get; }
		IBudgetPlanWriteRepository BudgetPlans { get; }
		IBudgetApproverWriteRepository BudgetApprovers { get; }
		ILedgerAccountRepository LedgerAccounts { get; }
		ILedgerAccountTypeRepository LedgerAccountTypes { get; }
		ICashoutCodeWriteRepository CashoutCodes { get; }
		ICashoutGroupRepository CashoutGroups { get; }

		// Files
		IStoredFilesRepository StoredFiles { get; }

		// Expense
		IInvoiceRepository Invoices { get; }
		IInvoiceFileRepository InvoiceFiles { get; }
		ISupplierRepository Suppliers { get; }
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

		// Events

		// Follow
		IFollowerRepository Followers { get; }

		// RBAC
		IRoleWriteRepository Roles { get; }
		IRolePermissionRepository RolePermissions { get; }
		IPermissionWriteRepository Permissions { get; }
		IUserRoleRepository UserRoles { get; }

		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}
