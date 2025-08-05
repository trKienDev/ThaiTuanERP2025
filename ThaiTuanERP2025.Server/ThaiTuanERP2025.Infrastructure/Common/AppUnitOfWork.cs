using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Repositories;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Finance.Repositories;
using ThaiTuanERP2025.Infrastructure.Account.Repositories;
using ThaiTuanERP2025.Infrastructure.Finance.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Common
{
	public class AppUnitOfWork : IUnitOfWork
	{
		private readonly ThaiTuanERP2025DbContext _context;

		// Account
		private IUserRepository? _users;
		public IUserRepository Users => _users ??= new UserRepository(_context);	

		private IDepartmentRepository? _departments;
		public IDepartmentRepository Departments => _departments ??= new DepartmentRepository(_context);

		private IGroupRepository? _groups;
		public IGroupRepository Groups => _groups ??= new GroupRepository(_context);

		private IUserGroupRepository? _userGroups;
		public IUserGroupRepository UserGroups => _userGroups ??= new UserGroupRepository(_context);

		// Finance
		private IBudgetGroupRepository? _budgetGroups;
		public IBudgetGroupRepository BudgetGroups => _budgetGroups ??= new BudgetGroupRepository(_context);

		private IBudgetPeriodRepository? _budgetPeriods;
		public IBudgetPeriodRepository BudgetPeriods => _budgetPeriods ??= new BudgetPeriodRepository(_context);

		private IBudgetPlanRepository? _budgetPlans;
		public IBudgetPlanRepository BudgetPlans => _budgetPlans ??= new BudgetPlanRepository(_context);

		private IBudgetCodeRepository? _budgetCodes;
		public IBudgetCodeRepository BudgetCodes => _budgetCodes ??= new BudgetCodeRepository(_context);

		private IBankAccountRepository? _bankAccounts;
		public IBankAccountRepository BankAccounts => _bankAccounts ??= new BankAccountRepository(_context);

		public AppUnitOfWork(ThaiTuanERP2025DbContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return _context.SaveChangesAsync(cancellationToken);
		}
	}
}
