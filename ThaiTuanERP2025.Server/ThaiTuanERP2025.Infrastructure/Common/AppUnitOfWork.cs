using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Repositories;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Common
{
	public class AppUnitOfWork : IUnitOfWork
	{
		private readonly ThaiTuanERP2025DbContext _context;
		public IUserRepository Users { get; }
		public IDepartmentRepository Departments { get; }
		public IGroupRepository Groups { get; }
		public IUserGroupRepository UserGroups { get; }

		public AppUnitOfWork(
			ThaiTuanERP2025DbContext context, 
			IUserRepository userRepository, 
			IDepartmentRepository departmentRepository,
			IGroupRepository groupRepository,
			IUserGroupRepository userGroupRepository
		) {
			_context = context ?? throw new ArgumentNullException(nameof(context));
			Users = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
			Departments = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
			Groups = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
			UserGroups = userGroupRepository ?? throw new ArgumentNullException(nameof(userGroupRepository));
		}

		public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return _context.SaveChangesAsync(cancellationToken);
		}
	}
}
