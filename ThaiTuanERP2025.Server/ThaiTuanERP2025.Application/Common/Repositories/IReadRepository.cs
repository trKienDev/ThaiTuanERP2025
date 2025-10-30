using System.Linq.Expressions;

namespace ThaiTuanERP2025.Application.Common.Repositories
{
	public interface IReadRepository<TEntity, TDto>
		where TEntity : class
		where TDto : class
	{
		Task<TDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
		Task<List<TDto>> GetAllAsync(
			Expression<Func<TEntity, bool>>? filter = null,
			string? keyword = null,
			Expression<Func<TEntity, object>>? orderBy = null,
			bool ascending = true,
			int? page = null,
			int? pageSize = null,
			CancellationToken cancellationToken = default
		);
		Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default);
	}
}
