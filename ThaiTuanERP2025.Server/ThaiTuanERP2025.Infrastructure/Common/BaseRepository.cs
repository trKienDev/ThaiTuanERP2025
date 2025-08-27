using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Common;
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

		public virtual Task<List<T>> ListAsync(Func<IQueryable<T>, IQueryable<T>> builder, bool asNoTracking = true, CancellationToken cancellationToken = default)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			var query = asNoTracking ? _dbSet.AsNoTracking() : _dbSet.AsQueryable();
			return builder(query).ToListAsync(cancellationToken);
		}
		public virtual Task<T?> SingleOrDefaultAsync(Func<IQueryable<T>, IQueryable<T>> builder, bool asNoTracking = true, CancellationToken cancellationToken = default)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			var query = asNoTracking ? _dbSet.AsNoTracking() : _dbSet.AsQueryable();
			return builder(query).SingleOrDefaultAsync(cancellationToken);
		}
		public virtual Task<T?> SingleOrDefaultIncludingAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = true, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));
			IQueryable<T> query = asNoTracking ? _dbSet.AsNoTracking() : _dbSet.AsQueryable();
			if (includes != null)
			{
				foreach (var include in includes)
					query = query.Include(include);
			}
			return query.SingleOrDefaultAsync(predicate, cancellationToken);
		}

		public IQueryable<T> Query(bool asNoTracking = true)
		{
			return asNoTracking ? _dbSet.AsNoTracking() : _dbSet.AsQueryable();
		}
		public IQueryable<T> Query(Expression<Func<T, bool>> predicate, bool asNoTracking = true)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));
			var query = asNoTracking ? _dbSet.AsNoTracking() : _dbSet.AsQueryable();
			return query.Where(predicate);
		}
		public IQueryable<T> QueryIncluding(bool asNoTracking = true, params Expression<Func<T, object>>[] includes) {
			IQueryable<T> query = asNoTracking ? _dbSet.AsNoTracking() : _dbSet.AsQueryable();
			if (includes != null)
			{
				foreach (var include in includes)
				{
					query = query.Include(include);
				}
			}
			return query;
		}

		public virtual async Task<T?> GetByIdAsync(Guid id)
		{
			return await _dbSet.FindAsync(id);
		}
		public virtual async Task<List<T>> GetAllAsync() {
			return await _dbSet.ToListAsync();
		}
		// filter by navigation
		public virtual async Task<List<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includes)
		{
			IQueryable<T> query = _dbSet.AsNoTracking();
			if (includes != null)
				foreach (var include in includes)
					query = query.Include(include);

			return await query.ToListAsync();
		}

		// filter by condition
		public virtual async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));
			return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
		}
		// filter by condition and navigation
		public virtual async Task<List<T>> FindIncludingAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));

			IQueryable<T> query = _dbSet.AsNoTracking().Where(predicate);
			if (includes != null)
				foreach (var include in includes.Distinct())
					query = query.Include(include);

			return await query.ToListAsync();
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

			// nếu entity là AuditableEntity thì xóa mềm
			if (entity is AuditableEntity auditable)
			{
				auditable.IsDeleted = true;
				auditable.DeletedDate = DateTime.UtcNow;
				// auditable.DeletedByUserId = _currentUserService.UserId; // nếu bạn đã có service người dùng hiện tại
				_dbSet.Update(entity); // update thay vì remove
			}
			else
			{
				// fallback: hard delete
				_dbSet.Remove(entity);
			}
		}
	}
}
