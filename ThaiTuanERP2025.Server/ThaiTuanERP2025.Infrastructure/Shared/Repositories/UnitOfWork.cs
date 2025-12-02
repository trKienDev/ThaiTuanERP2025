using ThaiTuanERP2025.Domain.Account.Repositories;
using ThaiTuanERP2025.Domain.Core.Repositories;
using ThaiTuanERP2025.Domain.Expense.Repositories;
using ThaiTuanERP2025.Domain.Finance.Repositories;
using ThaiTuanERP2025.Domain.Shared.Repositories;
using ThaiTuanERP2025.Domain.StoredFiles.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Shared.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ThaiTuanERP2025DbContext _dbContext;

		public UnitOfWork (
			ThaiTuanERP2025DbContext dbContext,

			IStoredFilesRepository storedFiles,
                        // Account
                        IUserWriteRepository users,
                        IUserManagerAssignmentWriteRepository userManagerAssignments,
			IDepartmentWriteRepository departments,
			IGroupRepository groups,
			IUserGroupRepository userGroups,

			// Finance
			IBudgetGroupWriteRepository budgetGroups,
			IBudgetPeriodWriteRepository budgetPeriods,
			IBudgetPlanWriteRepository budgetPlans,
			IBudgetPlanDetailWriteRepository budgetPlanDetails,
			IBudgetCodeWriteRepository budgetCodes,
			IBudgetApproverWriteRepository budgetApprovers,
			IBudgetTransactionWriteRepository budgetTransactions,
			ILedgerAccountWriteRepository ledgerAccounts,
			ILedgerAccountTypeWriteRepository ledgerAccountTypes,
			ICashoutCodeWriteRepository cashoutCodes,
			ICashoutGroupWriteRepository cashoutGroups,

			// Expense
			ISupplierWriteRepository suppliers,
			IOutgoingBankAccountRepository outgoingBankAccounts,
			IOutgoingPaymentRepository outgoingPayments,

			// Workflow	
			IExpenseStepTemplateWriteRepository expenseStepTemplates,
			IExpenseWorkflowTemplateWriteRepository expenseWorkflowTemplates,
			IExpenseStepInstanceWriteRepository expenseStepInstances,
			IExpenseWorkflowInstanceWriteRepository expenseWorkflowInstances,

			// Expense Payment
			IExpensePaymentWriteRepository expensePayments,
			IExpensePaymentItemsWriteRepository expensePaymentItems,
			IExpensePaymentAttachmentWriteRepository expensePaymentAttachments,
			IExpensePaymentCommentRepository expensePaymentComments,
			IExpensePaymentCommentTagRepository expensePaymentCommentTags,
			IExpensePaymentCommentAttachmentRepository expensePaymentCommentAttachments,

			// Core
			IFollowerWriteRepository followers,
			IUserNotificationWriteRepository userNotifications,
			IUserReminderWriteRepository userReminders,
			IOutboxMessageWriteRepository outboxMessages,

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
			BudgetPlanDetails = budgetPlanDetails;
			BudgetCodes = budgetCodes;
			BudgetApprovers = budgetApprovers;
			BudgetTransactions = budgetTransactions;

			LedgerAccountTypes = ledgerAccountTypes;
			LedgerAccounts = ledgerAccounts;
			CashoutCodes = cashoutCodes;
			CashoutGroups = cashoutGroups;

			Suppliers = suppliers;
			OutgoingBankAccounts = outgoingBankAccounts;
			OutgoingPayments = outgoingPayments;

			ExpenseStepTemplates = expenseStepTemplates;
			ExpenseWorkflowTemplates = expenseWorkflowTemplates;
			ExpenseStepInstances = expenseStepInstances;
			ExpenseWorkflowInstances = expenseWorkflowInstances;

			ExpensePayments = expensePayments;
			ExpensePaymentItems = expensePaymentItems;
			ExpensePaymentAttachments = expensePaymentAttachments;
			ExpensePaymentComments = expensePaymentComments;
			ExpensePaymentCommentAttachments = expensePaymentCommentAttachments;
			ExpensePaymentCommentTags = expensePaymentCommentTags;

			Followers = followers;
			UserNotifications = userNotifications;
			UserReminders = userReminders;
			OutboxMessages = outboxMessages;

			Roles = roles;
			Permissions = permissions;
			RolePermissions = rolePermissions;
			UserRoles = userRoles;
		}

		public IStoredFilesRepository StoredFiles { get; }
		// Account
		public IUserWriteRepository Users { get; }
		public IUserManagerAssignmentWriteRepository UserManagerAssignments { get; }
		public IDepartmentWriteRepository Departments { get; }
		public IGroupRepository Groups { get; }
		public IUserGroupRepository UserGroups { get; }

		// Finance
		public IBudgetGroupWriteRepository BudgetGroups { get; }
		public IBudgetPeriodWriteRepository BudgetPeriods { get; }
		public IBudgetPlanWriteRepository BudgetPlans { get; }
		public IBudgetPlanDetailWriteRepository BudgetPlanDetails { get; }
		public IBudgetCodeWriteRepository BudgetCodes { get; }
		public IBudgetApproverWriteRepository BudgetApprovers { get; }
		public ILedgerAccountWriteRepository LedgerAccounts { get; }
		public ILedgerAccountTypeWriteRepository LedgerAccountTypes { get; }
		public ICashoutCodeWriteRepository CashoutCodes { get; }
		public ICashoutGroupWriteRepository CashoutGroups { get; }
		public IBudgetTransactionWriteRepository BudgetTransactions { get;  }

		// Expense
		public ISupplierWriteRepository Suppliers { get; }
		public IOutgoingBankAccountRepository OutgoingBankAccounts { get; }
		public IOutgoingPaymentRepository OutgoingPayments { get; }

		// Workflow
		public IExpenseWorkflowTemplateWriteRepository ExpenseWorkflowTemplates { get; }
		public IExpenseStepTemplateWriteRepository ExpenseStepTemplates { get; }
		public IExpenseWorkflowInstanceWriteRepository ExpenseWorkflowInstances { get; }
		public IExpenseStepInstanceWriteRepository ExpenseStepInstances { get; }

		// Expense Payment
		public IExpensePaymentWriteRepository ExpensePayments { get; }
		public IExpensePaymentItemsWriteRepository ExpensePaymentItems { get; }
		public IExpensePaymentAttachmentWriteRepository ExpensePaymentAttachments { get; }
		public IExpensePaymentCommentRepository ExpensePaymentComments { get; }
		public IExpensePaymentCommentAttachmentRepository ExpensePaymentCommentAttachments { get; }
		public IExpensePaymentCommentTagRepository ExpensePaymentCommentTags { get; }

		// Core
		public IFollowerWriteRepository Followers { get; }
		public IUserNotificationWriteRepository UserNotifications { get; }
		public IUserReminderWriteRepository UserReminders { get; }
		public IOutboxMessageWriteRepository OutboxMessages { get;  }

		// RBAC
		public IUserRoleRepository UserRoles { get; }
		public IRoleWriteRepository Roles { get; }
		public IPermissionWriteRepository Permissions { get; }
		public IRolePermissionRepository RolePermissions { get; }

		public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return _dbContext.SaveChangesAsync(cancellationToken);
		}

		public Task<int> SaveChangesWithoutDispatchAsync(CancellationToken cancellationToken = default)
			=> _dbContext.SaveChangesWithoutDispatchAsync(cancellationToken);
	}
}
