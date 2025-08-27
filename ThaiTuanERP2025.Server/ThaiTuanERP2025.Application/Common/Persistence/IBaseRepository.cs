using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Common.Persistence
{
	public interface IBaseRepository<T> where T : class
	{
		Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
		Task<List<T>> ListAsync(Func<IQueryable<T>, IQueryable<T>> builder, bool asNoTracking = true, CancellationToken cancellationToken = default);
		Task<T?> SingleOrDefaultAsync(Func<IQueryable<T>, IQueryable<T>> builder, bool asNoTracking = true, CancellationToken cancellationToken = default);
		Task<T?> SingleOrDefaultIncludingAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = true, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes);

		// IQueryable<T> (ko materialize sớm) để còn compose filter/sort/paging/ProjectTo ở phía DB
		IQueryable<T> Query(bool asNoTracking = true);
		IQueryable<T> Query(Expression<Func<T, bool>> predicate, bool asNoTracking = true);
		IQueryable<T> QueryIncluding(bool asNoTracking = true, params Expression<Func<T, object>>[] includes);

		Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
		Task<List<T>> FindIncludingAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

		Task<T?> GetByIdAsync(Guid id);
		Task<List<T>> GetAllAsync();
		Task<List<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includes);

		Task<TDto?> GetByIdProjectedAsync<TDto>(Guid id, CancellationToken cancellationToken = default);

		Task AddAsync(T entity);
		
		void Update(T entity);

		void Delete(T entity);
	}
}
