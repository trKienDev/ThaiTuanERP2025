using System.Linq.Expressions;

namespace ThaiTuanERP2025.Domain.Shared.Repositories
{
	public interface IBaseWriteRepository<TEntity> where TEntity : class
	{
		Task<List<TResult>> ListAsync<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> builder, bool asNoTracking = true, CancellationToken cancellationToken = default);
		Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
		Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
		Task<TEntity?> SingleOrDefaultAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> builder, bool asNoTracking = true, CancellationToken cancellationToken = default);
		Task<TEntity?> SingleOrDefaultIncludingAsync(
		       Expression<Func<TEntity, bool>> predicate, bool asNoTracking = true, CancellationToken cancellationToken = default,
		       params Expression<Func<TEntity, object>>[] includes
		);
		// IQueryable<T> (ko materialize sớm) để còn compose filter/sort/paging/ProjectTo ở phía DB
		IQueryable<TEntity> Query(bool asNoTracking = true);
		IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = true);
		IQueryable<TEntity> QueryIncluding(bool asNoTracking = true, params Expression<Func<TEntity, object>>[] includes);
		IQueryable<TEntity> FindQueryable(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = true);

		Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
		Task<List<TEntity>> FindIncludingAsync(
			Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken, bool asNoTracking = true, 
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, params Expression<Func<TEntity, object>>[] includes
		);

		Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
		Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
		Task<List<TEntity>> GetAllIncludingAsync(CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] includes);

		Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
		Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

		void Update(TEntity entity);
		Task ReplaceRangeAsync(Expression<Func<TEntity, bool>> filter, IEnumerable<TEntity> newEntities, CancellationToken cancellationToken = default);

		void RemoveRange(IEnumerable<TEntity> entities);
		void Delete(TEntity entity);
	}
}
