using Drive.Domain.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;

namespace Drive.Infrastructure.Persistence.Shared.Repositories
{
	public class BaseWriteRepository<TEntity> : IBaseWriteRepository<TEntity> where TEntity : class
	{
		protected readonly DbContext _context;
		protected readonly DbSet<TEntity> _dbSet;
		public BaseWriteRepository(ThaiTuanERP2025DriveDbContext context, IConfigurationProvider configurationProvider)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_dbSet = _context.Set<TEntity>();
		}

		public virtual Task<List<TResult>> ListAsync<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> builder, bool asNoTracking = true, CancellationToken cancellationToken = default)
		{
			var query = asNoTracking ? _dbSet.AsNoTracking() : _dbSet.AsQueryable();
			return builder(query).ToListAsync(cancellationToken);
		}

		public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));
			return _dbSet.AsNoTracking().CountAsync(predicate, cancellationToken);
		}

		public async Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));
			return await _dbSet.AnyAsync(predicate, cancellationToken);
		}

		public virtual Task<TEntity?> SingleOrDefaultAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> builder, bool asNoTracking = true, CancellationToken cancellationToken = default)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));
			var query = asNoTracking ? _dbSet.AsNoTracking() : _dbSet.AsQueryable();
			return builder(query).SingleOrDefaultAsync(cancellationToken);
		}
		public virtual Task<TEntity?> SingleOrDefaultIncludingAsync(
			Expression<Func<TEntity, bool>> predicate, bool asNoTracking = true, CancellationToken cancellationToken = default,
			params Expression<Func<TEntity, object>>[] includes
		)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));
			IQueryable<TEntity> query = asNoTracking ? _dbSet.AsNoTracking() : _dbSet.AsQueryable();
			if (includes != null)
			{
				foreach (var include in includes)
					query = query.Include(include);
			}
			return query.SingleOrDefaultAsync(predicate, cancellationToken);
		}

		public IQueryable<TEntity> Query(bool asNoTracking = true)
		{
			return asNoTracking ? _dbSet.AsNoTracking() : _dbSet.AsQueryable();
		}
		public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = true)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));
			var query = asNoTracking ? _dbSet.AsNoTracking() : _dbSet.AsQueryable();
			return query.Where(predicate);
		}
		public IQueryable<TEntity> QueryIncluding(bool asNoTracking = true, params Expression<Func<TEntity, object>>[] includes)
		{
			IQueryable<TEntity> query = asNoTracking ? _dbSet.AsNoTracking() : _dbSet.AsQueryable();
			if (includes != null)
			{
				foreach (var include in includes)
				{
					query = query.Include(include);
				}
			}
			return query;
		}
		public IQueryable<TEntity> FindQueryable(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = true)
		{
			if (predicate is null) throw new ArgumentNullException(nameof(predicate));
			return Query(asNoTracking).Where(predicate);
		}

		public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
		{
			return await _dbSet.FindAsync(new object?[] { id }, cancellationToken);
		}
		public virtual async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
		{
			return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
		}
		// filter by navigation
		public virtual async Task<List<TEntity>> GetAllIncludingAsync(CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] includes)
		{
			IQueryable<TEntity> query = _dbSet.AsNoTracking();
			if (includes != null)
				foreach (var include in includes)
					query = query.Include(include);

			return await query.ToListAsync();
		}

		// filter by condition
		public virtual async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));
			return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
		}
		// filter by condition and navigation
		public virtual async Task<List<TEntity>> FindIncludingAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken, bool asNoTracking = true,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, params Expression<Func<TEntity, object>>[] includes)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));

			IQueryable<TEntity> query = asNoTracking ? _dbSet.AsNoTracking() : _dbSet.AsQueryable();

			if (includes != null && includes.Length > 0)
				foreach (var include in includes.Distinct())
					query = query.Include(include);

			if (orderBy != null) query = orderBy(query);

			return await query.Where(predicate).ToListAsync(cancellationToken);
		}

		public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
		{
			if (entity == null) throw new ArgumentNullException(nameof(entity));
			await _dbSet.AddAsync(entity, cancellationToken);
		}
		public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
		{
			if (entities == null) throw new ArgumentNullException(nameof(entities));
			await _dbSet.AddRangeAsync(entities, cancellationToken);
		}

		public void Update(TEntity entity)
		{
			if (entity == null) throw new ArgumentNullException(nameof(entity));
			_dbSet.Update(entity);
		}
		public async Task ReplaceRangeAsync(Expression<Func<TEntity, bool>> filter, IEnumerable<TEntity> newEntities, CancellationToken cancellationToken = default)
		{
			var existing = _dbSet.Where(filter);
			_dbSet.RemoveRange(existing);
			await _dbSet.AddRangeAsync(newEntities, cancellationToken);
		}

		public void Delete(TEntity entity)
		{
			if (entity == null) throw new ArgumentNullException(nameof(entity));

			_dbSet.Remove(entity);
		}

		public void RemoveRange(IEnumerable<TEntity> entities)
		{
			if (entities == null) throw new ArgumentNullException(nameof(entities));
			_dbSet.RemoveRange(entities);
		}
	}
}
