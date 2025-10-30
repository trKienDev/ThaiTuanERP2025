using Microsoft.Identity.Client;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Expense.Repositories;
using ThaiTuanERP2025.Application.Files.Repositories;
using ThaiTuanERP2025.Application.Finance.Budgets.Repositories;
using ThaiTuanERP2025.Application.Followers.Repositories;
using ThaiTuanERP2025.Application.Notifications.Repositories;
using ThaiTuanERP2025.Domain.Account.Repositories;
using ThaiTuanERP2025.Domain.Finance.Repositories;
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
			IUserRepository users,
			IUserManagerAssignmentRepository userManagerAssignments,
			IDepartmentRepository departments,
			IGroupRepository groups,
			IUserGroupRepository userGroups,

			// Finance
			IBudgetGroupRepository budgetGroups,
			IBudgetPeriodRepository budgetPeriods,
			IBudgetPlanRepository budgetPlans,
			IBudgetCodeRepository budgetCodes,
			ILedgerAccountRepository ledgerAccounts,
			ILedgerAccountTypeRepository ledgerAccountTypes,
			ITaxRepository taxes,
			IWithholdingTaxTypeRepository withholdingTaxTypes,
			ICashoutCodeRepository cashoutCodes,
			ICashoutGroupRepository cashoutGroups,

			// Expense
			IInvoiceRepository invoices,
			IInvoiceLineRepository invoiceLines,
			IInvoiceFileRepository invoiceFiles,
			IInvoiceFollowerRepository invoiceFollowers,
			ISupplierRepository suppliers,
			IBankAccountRepository bankAccounts,
			IOutgoingBankAccountRepository outgoingBankAccounts,
			IOutgoingPaymentRepository outgoingPayments,

			// Workflow	
			IApprovalStepTemplateRepository approvalStepTemplates,
			IApprovalWorkflowTemplateRepository approvalWorkflowTemplates,
			IApprovalStepInstanceRepository approvalStepInstances,
			IApprovalWorkflowInstanceRepository approvalWorkflowInstances,

			// Expense Payment
			IExpensePaymentRepository expensePayments,
			IExpensePaymentCommentRepository expensePaymentComments,
			IExpensePaymentCommentTagRepository expensePaymentCommentTags,
			IExpensePaymentCommentAttachmentRepository expensePaymentCommentAttachments,

			// Notification
			INotificationRepository notifications,
			ITaskReminderRepository taskReminders,

			// Follow
			IFollowerRepository followers,

			// RBAC
			IRoleRepository roles,
			IPermissionRepository permissions,
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
			Taxes = taxes;
			WithholdingTaxTypes = withholdingTaxTypes;
			CashoutCodes = cashoutCodes;
			CashoutGroups = cashoutGroups;

			Invoices = invoices;
			InvoiceLines = invoiceLines;
			InvoiceFiles = invoiceFiles;
			InvoiceFollowers = invoiceFollowers;
			Suppliers = suppliers;
			BankAccounts = bankAccounts;
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
		public IUserRepository Users { get; }
		public IUserManagerAssignmentRepository UserManagerAssignments { get; }
		public IDepartmentRepository Departments { get; }
		public IGroupRepository Groups { get; }
		public IUserGroupRepository UserGroups { get; }

		// Finance
		public IBudgetGroupRepository BudgetGroups { get; }
		public IBudgetPeriodRepository BudgetPeriods { get; }
		public IBudgetPlanRepository BudgetPlans { get; }
		public IBudgetCodeRepository BudgetCodes { get; }
		public ILedgerAccountRepository LedgerAccounts { get; }
		public ILedgerAccountTypeRepository LedgerAccountTypes { get; }
		public ITaxRepository Taxes { get; }
		public IWithholdingTaxTypeRepository WithholdingTaxTypes { get; }
		public ICashoutCodeRepository CashoutCodes { get; }
		public ICashoutGroupRepository CashoutGroups { get; }

		// Expense
		public IInvoiceRepository Invoices { get; }
		public IInvoiceLineRepository InvoiceLines { get; }
		public IInvoiceFileRepository InvoiceFiles { get; }
		public IInvoiceFollowerRepository InvoiceFollowers { get; }
		public ISupplierRepository Suppliers { get; }
		public IBankAccountRepository BankAccounts { get; }
		public IOutgoingBankAccountRepository OutgoingBankAccounts { get; }
		public IOutgoingPaymentRepository OutgoingPayments { get; }

		// Workflow
		public IApprovalWorkflowTemplateRepository ApprovalWorkflowTemplates { get; }
		public IApprovalStepTemplateRepository ApprovalStepTemplates { get; }
		public IApprovalWorkflowInstanceRepository ApprovalWorkflowInstances { get; }
		public IApprovalStepInstanceRepository ApprovalStepInstances { get; }

		// Expense Payment
		public IExpensePaymentRepository ExpensePayments { get; }
		public IExpensePaymentCommentRepository ExpensePaymentComments { get; }
		public IExpensePaymentCommentAttachmentRepository ExpensePaymentCommentAttachments { get; }
		public IExpensePaymentCommentTagRepository ExpensePaymentCommentTags { get; }

		// Notification
		public INotificationRepository Notifications { get; }
		public ITaskReminderRepository TaskReminders { get; }

		// Followers
		public IFollowerRepository Followers { get; }

		// RBAC
		public IUserRoleRepository UserRoles { get; }
		public IRoleRepository Roles { get; }
		public IPermissionRepository Permissions { get; }
		public IRolePermissionRepository RolePermissions { get; }

		public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return _dbContext.SaveChangesAsync(cancellationToken);
		}
	}
}
