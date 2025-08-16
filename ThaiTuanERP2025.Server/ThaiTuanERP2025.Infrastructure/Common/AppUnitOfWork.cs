using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Repositories;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Finance.Repositories;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Infrastructure.Account.Repositories;
using ThaiTuanERP2025.Infrastructure.Finance.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Common
{
	public class AppUnitOfWork : IUnitOfWork
	{
		private readonly ThaiTuanERP2025DbContext _dbContext;

		public AppUnitOfWork(
			ThaiTuanERP2025DbContext dbContext,

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
			IBankAccountReadRepository bankAccountRead
		) {
			_dbContext = dbContext;

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
		}

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

		public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return _dbContext.SaveChangesAsync(cancellationToken);
		}
	}
}
