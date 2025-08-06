using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Account;
using ThaiTuanERP2025.Domain.Account.Entities;

namespace ThaiTuanERP2025.Application.Account.Repositories
{
	public interface IUserRepository : IBaseRepository<User>
	{
		Task<User?> GetByUsernameAsync(string username);
		Task<User?> GetByEmployeeCode(string employeeCode);
	}
}
