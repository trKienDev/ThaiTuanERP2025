using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Common
{
	public class BaseRepository<T> : IBaseRepository<T> where T : class
	{
		protected readonly DbContext _context;
		protected readonly DbSet<T> _dbSet;
		public BaseRepository(ThaiTuanERP2025DbContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_dbSet = _context.Set<T>();
		}

		public virtual async Task<T?> GetByIdAsync(Guid id)
		{
			return await _dbSet.FindAsync(id);
		}

		public virtual async Task<List<T>> GetAllAsync() {
			return await _dbSet.ToListAsync();
		}

		public async Task AddAsync(T entity)
		{
			if (entity == null) throw new ArgumentNullException(nameof(entity));
			await _dbSet.AddAsync(entity);
		}

		public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));
			return await _dbSet.AnyAsync(predicate);
		}

		public void Update(T entity)
		{
			if (entity == null) throw new ArgumentNullException(nameof(entity));
			_dbSet.Update(entity);
		}

		public void Delete(T entity)
		{
			if (entity == null) throw new ArgumentNullException(nameof(entity));
			_dbSet.Remove(entity);
		}
	}
}
