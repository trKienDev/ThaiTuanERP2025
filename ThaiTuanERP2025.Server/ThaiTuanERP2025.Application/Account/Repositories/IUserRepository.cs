using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Account;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Application.Account.Repositories
{
	public interface IUserRepository
	{
		Task<User?> GetByIdAsync(Guid id);
		Task<User?> GetByEmployeeCode(string employeeCode);
		Task<User?> GetByUsernameAsync(string username);
		Task<List<User>> GetAllAsync();

		Task AddAsync(User user);
		Task UpdateAysnc(User user);
		void Remove(User user);

	}
}
