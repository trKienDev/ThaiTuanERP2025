using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Repositories;
using ThaiTuanERP2025.Application.Expense.Repositories;
using ThaiTuanERP2025.Application.Files.Repositories;
using ThaiTuanERP2025.Application.Finance.Repositories;
using ThaiTuanERP2025.Application.Partner.Repositories;

namespace ThaiTuanERP2025.Application.Common.Persistence
{
	public interface IUnitOfWork
	{
		// Define the DbContext type

		// Account
		IUserRepository Users { get; }
		IDepartmentRepository Departments { get; }
		IGroupRepository Groups { get; }
		IUserGroupRepository UserGroups { get; }
		IBankAccountRepository BankAccounts { get; }
		IBankAccountReadRepository BankAccountRead { get; }

		// Partner
		ISupplierRepository Suppliers { get; }

		// Finance
		IBudgetCodeRepository BudgetCodes { get; }
		IBudgetGroupRepository BudgetGroups { get; }
		IBudgetPeriodRepository BudgetPeriods { get; }
		IBudgetPlanRepository BudgetPlans { get; }
		
		IPartnerBankAccountRepository PartnerBankAccounts { get; }
		ILedgerAccountRepository LedgerAccounts { get; }
		ILedgerAccountTypeRepository LedgerAccountTypes { get; }
		ITaxRepository Taxes { get; }
		IWithholdingTaxTypeRepository WithholdingTaxTypes { get; }
		ICashoutCodeRepository CashoutCodes { get; }
		ICashoutGroupRepository CashoutGroups { get; }
		IStoredFilesRepository StoredFiles { get; }

		// Expense
		IInvoiceRepository Invoices { get;  }
		IInvoiceLineRepository InvoiceLines { get; }
		IInvoiceFileRepository InvoiceFiles { get; }
		IInvoiceFollowerRepository InvoiceFollowers { get; }

		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}
