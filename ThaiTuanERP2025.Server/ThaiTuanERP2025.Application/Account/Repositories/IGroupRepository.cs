using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Application.Account.Repositories
{
	public interface IGroupRepository
	{
		Task<Group?> GetByIdAsync(Guid id);
		Task<List<Group>> GetAllAsync();
		Task AddAsync(Group group);
		Task DeleteAsync (Group group);
		Task<bool> ExistAsync(Guid id);
	}
}
