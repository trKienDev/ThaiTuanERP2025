using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Account.Repositories;
using ThaiTuanERP2025.Domain.Alerts.Repositories;
using ThaiTuanERP2025.Domain.Expense.Repositories;
using ThaiTuanERP2025.Domain.Files.Repositories;
using ThaiTuanERP2025.Domain.Finance.Repositories;
using ThaiTuanERP2025.Domain.Followers.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Common
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ThaiTuanERP2025DbContext _dbContext;

		public UnitOfWork(
			ThaiTuanERP2025DbContext dbContext,

			IStoredFilesRepository storedFiles, 

			// Account
			IUserWriteRepository users,
			IUserManagerAssignmentRepository userManagerAssignments,
			IDepartmentWriteRepository departments,
			IGroupRepository groups,
			IUserGroupRepository userGroups,

			// Finance
			IBudgetGroupWriteRepository budgetGroups,
			IBudgetPeriodWriteRepository budgetPeriods,
			IBudgetPlanRepository budgetPlans,
			IBudgetCodeWriteRepository budgetCodes,
			ILedgerAccountRepository ledgerAccounts,
			ILedgerAccountTypeRepository ledgerAccountTypes,
			ICashoutCodeWriteRepository cashoutCodes,
			ICashoutGroupRepository cashoutGroups,

			// Expense
			IInvoiceRepository invoices,
			IInvoiceFileRepository invoiceFiles,
			ISupplierRepository suppliers,
			IOutgoingBankAccountRepository outgoingBankAccounts,
			IOutgoingPaymentRepository outgoingPayments,

			// Workflow	
			IExpenseStepTemplateRepository approvalStepTemplates,
			IExpenseWorkflowTemplateRepository approvalWorkflowTemplates,
			IExpenseStepInstanceRepository approvalStepInstances,
			IExpenseWorkflowInstanceRepository approvalWorkflowInstances,

			// Expense Payment
			IExpensePaymentRepository expensePayments,
			IExpensePaymentCommentRepository expensePaymentComments,
			IExpensePaymentCommentTagRepository expensePaymentCommentTags,
			IExpensePaymentCommentAttachmentRepository expensePaymentCommentAttachments,

			// Notification
			INotificationWriteRepository notifications,
			ITaskReminderWriteRepository taskReminders,

			// Follow
			IFollowerRepository followers,

			// RBAC
			IRoleWriteRepository roles,
			IPermissionWriteRepository permissions,
			IRolePermissionRepository rolePermissions,
			IUserRoleRepository userRoles
		)
		{
			_dbContext = dbContext;

			StoredFiles = storedFiles;

			Users = users;
			UserManagerAssignments = userManagerAssignments;
			Departments = departments;
			Groups = groups;
			UserGroups = userGroups;

			BudgetGroups = budgetGroups;
			BudgetPeriods = budgetPeriods;
			BudgetPlans = budgetPlans;
			BudgetCodes = budgetCodes;

			LedgerAccountTypes = ledgerAccountTypes;
			LedgerAccounts = ledgerAccounts;
			CashoutCodes = cashoutCodes;
			CashoutGroups = cashoutGroups;

			Invoices = invoices;
			InvoiceFiles = invoiceFiles;
			Suppliers = suppliers;
			OutgoingBankAccounts = outgoingBankAccounts;
			OutgoingPayments = outgoingPayments;

			ApprovalStepTemplates = approvalStepTemplates;
			ApprovalWorkflowTemplates = approvalWorkflowTemplates;
			ApprovalStepInstances = approvalStepInstances;
			ApprovalWorkflowInstances = approvalWorkflowInstances;

			ExpensePayments = expensePayments;
			ExpensePaymentComments = expensePaymentComments;
			ExpensePaymentCommentAttachments = expensePaymentCommentAttachments;
			ExpensePaymentCommentTags = expensePaymentCommentTags;

			Notifications = notifications;
			TaskReminders = taskReminders;

			Followers = followers;

			Roles = roles;
			Permissions = permissions;
			RolePermissions = rolePermissions;
			UserRoles = userRoles;
		}

		public IStoredFilesRepository StoredFiles { get; }
		// Account
		public IUserWriteRepository Users { get; }
		public IUserManagerAssignmentRepository UserManagerAssignments { get; }
		public IDepartmentWriteRepository Departments { get; }
		public IGroupRepository Groups { get; }
		public IUserGroupRepository UserGroups { get; }

		// Finance
		public IBudgetGroupWriteRepository BudgetGroups { get; }
		public IBudgetPeriodWriteRepository BudgetPeriods { get; }
		public IBudgetPlanRepository BudgetPlans { get; }
		public IBudgetCodeWriteRepository BudgetCodes { get; }
		public ILedgerAccountRepository LedgerAccounts { get; }
		public ILedgerAccountTypeRepository LedgerAccountTypes { get; }
		public ICashoutCodeWriteRepository CashoutCodes { get; }
		public ICashoutGroupRepository CashoutGroups { get; }

		// Expense
		public IInvoiceRepository Invoices { get; }
		public IInvoiceFileRepository InvoiceFiles { get; }
		public ISupplierRepository Suppliers { get; }
		public IOutgoingBankAccountRepository OutgoingBankAccounts { get; }
		public IOutgoingPaymentRepository OutgoingPayments { get; }

		// Workflow
		public IExpenseWorkflowTemplateRepository ApprovalWorkflowTemplates { get; }
		public IExpenseStepTemplateRepository ApprovalStepTemplates { get; }
		public IExpenseWorkflowInstanceRepository ApprovalWorkflowInstances { get; }
		public IExpenseStepInstanceRepository ApprovalStepInstances { get; }

		// Expense Payment
		public IExpensePaymentRepository ExpensePayments { get; }
		public IExpensePaymentCommentRepository ExpensePaymentComments { get; }
		public IExpensePaymentCommentAttachmentRepository ExpensePaymentCommentAttachments { get; }
		public IExpensePaymentCommentTagRepository ExpensePaymentCommentTags { get; }

		// Notification
		public INotificationWriteRepository Notifications { get; }
		public ITaskReminderWriteRepository TaskReminders { get; }

		// Followers
		public IFollowerRepository Followers { get; }

		// RBAC
		public IUserRoleRepository UserRoles { get; }
		public IRoleWriteRepository Roles { get; }
		public IPermissionWriteRepository Permissions { get; }
		public IRolePermissionRepository RolePermissions { get; }

		public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return _dbContext.SaveChangesAsync(cancellationToken);
		}
	}
}
