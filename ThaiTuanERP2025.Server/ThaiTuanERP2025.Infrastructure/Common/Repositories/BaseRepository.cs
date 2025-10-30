using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Common.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Common.Repositories
{
	public class BaseRepository<T> : IBaseRepository<T> where T : class
	{
		protected readonly DbContext _context;
		protected readonly DbSet<T> _dbSet;
		private readonly IConfigurationProvider _configurationProvider;
		public BaseRepository(ThaiTuanERP2025DbContext context, IConfigurationProvider configurationProvider)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_dbSet = _context.Set<T>();
			_configurationProvider = configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));
		}

		public virtual Task<List<TResult>> ListAsync<TResult>(Func<IQueryable<T>, IQueryable<TResult>> builder, bool asNoTracking = true, CancellationToken cancellationToken = default)
		{
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
		public IQueryable<T> QueryIncluding(bool asNoTracking = true, params Expression<Func<T, object>>[] includes)
		{
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

		public virtual Task<bool> ExistAsync(Guid id, CancellationToken cancellationToken = default)
		{
			return _dbSet.AsNoTracking().AnyAsync(e => EF.Property<Guid>(e, "Id") == id, cancellationToken);
		}
		public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
		{
			return await _dbSet.FindAsync(new object?[] { id }, cancellationToken);
		}
		public virtual async Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
		{
			return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
		}
		// filter by navigation
		public virtual async Task<List<T>> GetAllIncludingAsync(CancellationToken cancellationToken, params Expression<Func<T, object>>[] includes)
		{
			IQueryable<T> query = _dbSet.AsNoTracking();
			if (includes != null)
				foreach (var include in includes)
					query = query.Include(include);

			return await query.ToListAsync();
		}

		// filter by condition
		public virtual async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));
			return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
		}
		// filter by condition and navigation
		public virtual async Task<List<T>> FindIncludingAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken, bool asNoTracking = true,
			Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, params Expression<Func<T, object>>[] includes)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));

			IQueryable<T> query = asNoTracking ? _dbSet.AsNoTracking() : _dbSet.AsQueryable();

			if (includes != null && includes.Length > 0)
				foreach (var include in includes.Distinct())
					query = query.Include(include);

			if (orderBy != null)
				query = orderBy(query);

			return await query.Where(predicate).ToListAsync(cancellationToken);
		}
		public IQueryable<T> FindQueryable(Expression<Func<T, bool>> predicate, bool asNoTracking = true)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));
			var query = asNoTracking ? _dbSet.AsNoTracking() : _dbSet.AsQueryable();
			return query.Where(predicate);
		}

		public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
		{
			if (entity == null) throw new ArgumentNullException(nameof(entity));
			await _dbSet.AddAsync(entity, cancellationToken);
		}
		public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
		{
			if (entities == null) throw new ArgumentNullException(nameof(entities));
			await _dbSet.AddRangeAsync(entities, cancellationToken);
		}

		public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));
			return await _dbSet.AnyAsync(predicate, cancellationToken);
		}

		public virtual Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));
			return _dbSet.AsNoTracking().CountAsync(predicate, cancellationToken);
		}

		public void Update(T entity)
		{
			if (entity == null) throw new ArgumentNullException(nameof(entity));
			_dbSet.Update(entity);
		}
		public async Task ReplaceRangeAsync(Expression<Func<T, bool>> filter, IEnumerable<T> newEntities, CancellationToken cancellationToken = default)
		{
			var existing = _dbSet.Where(filter);
			_dbSet.RemoveRange(existing);
			await _dbSet.AddRangeAsync(newEntities, cancellationToken);
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

		public void RemoveRange(IEnumerable<T> entities)
		{
			if (entities == null) throw new ArgumentNullException(nameof(entities));
			_dbSet.RemoveRange(entities);
		}


		public async Task<TDto?> GetByIdProjectedAsync<TDto>(Guid id, CancellationToken cancellationToken = default)
		{
			return await Query() // AsNoTracking mặc định
				.Where(e => EF.Property<Guid>(e, "Id") == id)
				.ProjectTo<TDto>(_configurationProvider)
				.SingleOrDefaultAsync(cancellationToken);
		}

		public Task<List<TDto>> ListProjectedAsync<TDto>(Func<IQueryable<T>, IQueryable<TDto>> builder, bool asNoTracking = true, CancellationToken cancellationToken = default)
		{
			var query = asNoTracking ? _dbSet.AsNoTracking() : _dbSet.AsQueryable();
			return builder(query).ToListAsync(cancellationToken); // materialize trong repo
		}
	}
}
