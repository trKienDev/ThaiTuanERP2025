using System.Linq.Expressions;

namespace ThaiTuanERP2025.Application.Common.Repositories
{
	public interface IBaseReadRepository<TEntity, TDto>
		where TEntity : class 
		where TDto : class
	{
		IQueryable<TEntity> Query(bool asNoTracking = true);
		Task<TDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
		Task<List<TDto>> GetAllAsync(
			Expression<Func<TEntity, bool>>? filter = null, string? keyword = null,
			Expression<Func<TEntity, object>>? orderBy = null,
			bool ascending = true, int? page = null, int? pageSize = null,
			CancellationToken cancellationToken = default
		);
		Task<List<TDto>> ListAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> builder, bool asNoTracking = true, CancellationToken cancellationToken = default);
		Task<TDto?> GetByIdProjectedAsync(Guid id, CancellationToken cancellationToken = default);
		Task<List<TDto>> ListProjectedAsync(Func<IQueryable<TEntity>, IQueryable<TDto>> builder, bool asNoTracking = true, CancellationToken cancellationToken = default);
	}
}
