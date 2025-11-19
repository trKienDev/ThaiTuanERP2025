using ThaiTuanERP2025.Domain.Shared.Entities;
using ThaiTuanERP2025.Domain.Shared.Interfaces;

namespace ThaiTuanERP2025.Domain.Shared.Extensions
{
	public static class QueryableExtensions
	{
		public static IQueryable<TEntity> Active<TEntity>(this IQueryable<TEntity> query) where TEntity : AuditableEntity, IActiveEntity
		{
			return query.Where(x => x.IsActive && !x.IsDeleted);
		}
	}
}
