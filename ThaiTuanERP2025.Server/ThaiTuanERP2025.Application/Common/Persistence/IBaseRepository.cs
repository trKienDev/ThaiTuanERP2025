using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Common.Persistence
{
	public interface IBaseRepository<T> where T : class
	{
		Task<T?> GetByIdAsync(Guid id);
		Task<List<T>> GetAllAsync();
		Task AddAsync(T entity);
		void Update(T entity);
		void Delete(T entity);
	}
}
