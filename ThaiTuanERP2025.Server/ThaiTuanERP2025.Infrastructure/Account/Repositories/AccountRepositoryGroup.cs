using ThaiTuanERP2025.Domain.Account.Repositories;

namespace ThaiTuanERP2025.Infrastructure.Account.Repositories
{
	public class AccountRepositoryGroup
	{
		public AccountRepositoryGroup(
			IUserWriteRepository users,
			IUserManagerAssignmentRepository userManagerAssignments
		) {
			Users = users;
		}
		public IUserWriteRepository Users { get; }
	}
}
