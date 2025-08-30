using ThaiTuanERP2025.Application.Account.Repositories;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Expense.Repositories;
using ThaiTuanERP2025.Application.Files.Repositories;
using ThaiTuanERP2025.Application.Finance.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Common
{
	public class AppUnitOfWork : IUnitOfWork
	{
		private readonly ThaiTuanERP2025DbContext _dbContext;

		public AppUnitOfWork(
			ThaiTuanERP2025DbContext dbContext,

			IStoredFilesRepository storedFiles, 

			// Account
			IUserRepository users,
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

			// Approval
			IApprovalFlowDefinitionRepository approvalFlowDefinitions,
			IApprovalFlowInstanceRepository approvalFlowInstances,
			IApprovalActionRepository approvalActions

		) {
			_dbContext = dbContext;

			StoredFiles = storedFiles;

			Users = users;
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

			ApprovalFlowDefinitions = approvalFlowDefinitions;
			ApprovalFlowInstances = approvalFlowInstances;
			ApprovalActions = approvalActions;
		}

		public IStoredFilesRepository StoredFiles { get; }
		// Account
		public IUserRepository Users { get; }
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

		// Approval
		public IApprovalFlowDefinitionRepository ApprovalFlowDefinitions { get; }
		public IApprovalFlowInstanceRepository ApprovalFlowInstances { get; }
		public IApprovalActionRepository ApprovalActions { get; }

		public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return _dbContext.SaveChangesAsync(cancellationToken);
		}
	}
}
