using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Application.Account.Repositories
{
	public interface IGroupRepository : IBaseRepository<Group>
	{
		Task<bool> ExistAsync(Guid id);
	}
}
