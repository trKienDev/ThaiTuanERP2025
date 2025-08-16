using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Repositories;
using ThaiTuanERP2025.Application.Finance.Repositories;

namespace ThaiTuanERP2025.Application.Common.Persistence
{
	public interface IUnitOfWork
	{
		// Define the DbContext type
		IUserRepository Users { get; }
		IDepartmentRepository Departments { get; }
		IGroupRepository Groups { get; }
		IUserGroupRepository UserGroups { get; }
		IBankAccountRepository BankAccounts { get; }
		IBankAccountReadRepository BankAccountRead { get; }
		IBudgetCodeRepository BudgetCodes { get; }
		IBudgetGroupRepository BudgetGroups { get; }
		IBudgetPeriodRepository BudgetPeriods { get; }
		IBudgetPlanRepository BudgetPlans { get; }
		ISupplierRepository Suppliers { get; }

		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
		//Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess);
		//void Dispose();
		//void Dispose(bool disposing);
		//bool HasChanges();
		//int SaveChanges();
		//int SaveChanges(bool acceptAllChangesOnSuccess);
		//void Clear();
		//void RejectChanges();
		//void AcceptAllChanges();
		//void AcceptAllChanges(bool acceptAllChangesOnSuccess);
	}
}
