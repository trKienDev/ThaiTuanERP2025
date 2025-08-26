using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Repositories;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Files.Repositories;
using ThaiTuanERP2025.Application.Finance.Repositories;
using ThaiTuanERP2025.Application.Partner.Repositories;
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
			IBankAccountRepository bankAccounts,
			IBankAccountReadRepository bankAccountRead,
			ISupplierRepository suppliers,
			ILedgerAccountRepository ledgerAccounts,
			ILedgerAccountTypeRepository ledgerAccountTypes,
			ITaxRepository taxes,
			ICashoutCodeRepository cashoutCodes,
			ICashoutGroupRepository cashoutGroups,

			// Partner
			IPartnerBankAccountRepository partnerBankAccount
			
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

			BankAccounts = bankAccounts;
			BankAccountRead = bankAccountRead;
			Suppliers = suppliers;

			LedgerAccountTypes = ledgerAccountTypes;
			LedgerAccounts = ledgerAccounts;
			Taxes = taxes;
			CashoutCodes = cashoutCodes;
			CashoutGroups = cashoutGroups;

			PartnerBankAccounts = partnerBankAccount;
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
		public IBankAccountRepository BankAccounts { get; }
		public IBankAccountReadRepository BankAccountRead { get; }
		public ISupplierRepository Suppliers { get; }
		public IPartnerBankAccountRepository PartnerBankAccounts { get; }
		public ILedgerAccountRepository LedgerAccounts { get; }
		public ILedgerAccountTypeRepository LedgerAccountTypes { get; }
		public ITaxRepository Taxes { get; }
		public ICashoutCodeRepository CashoutCodes { get; }
		public ICashoutGroupRepository CashoutGroups { get; }

		public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return _dbContext.SaveChangesAsync(cancellationToken);
		}
	}
}
