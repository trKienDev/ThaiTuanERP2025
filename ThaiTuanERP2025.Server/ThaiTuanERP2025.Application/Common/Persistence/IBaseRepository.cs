using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Common.Persistence
{
	public interface IBaseRepository<T> where T : class
	{
		Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

		Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
		Task<List<T>> FindIncludingAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

		Task<T?> GetByIdAsync(Guid id);
		Task<List<T>> GetAllAsync();
		Task<List<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includes);

		Task AddAsync(T entity);
		
		void Update(T entity);
		void Delete(T entity);
	}
}
