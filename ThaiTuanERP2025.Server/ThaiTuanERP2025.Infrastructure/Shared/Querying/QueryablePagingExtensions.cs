using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ThaiTuanERP2025.Application.Shared.Models;

namespace ThaiTuanERP2025.Infrastructure.Shared.Querying
{
	public static class QueryablePagingExtensions
	{
		public static IOrderedQueryable<T> ApplySorting<T>(
			this IQueryable<T> query,
			string? sort,
			IDictionary<string, Expression<Func<T, object>>> map,
			string defaultField
		){
			var dir = "asc";
			var field = defaultField;

			if (!string.IsNullOrWhiteSpace(sort))
			{
				var parts = sort.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
				if (parts.Length > 0) field = parts[0].ToLowerInvariant();
				if (parts.Length > 1) dir = parts[1].ToLowerInvariant() is "desc" ? "desc" : "asc";
			}

			if (!map.TryGetValue(field, out var keySelector))
				keySelector = map[defaultField];

			return dir == "desc" ? query.OrderByDescending(keySelector) : query.OrderBy(keySelector);
		}

		public static async Task<PagedResult<TDto>> ToPagedResultAsync<TEntity, TDto>(
			this IOrderedQueryable<TEntity> ordered,
			int pageIndex,
			int pageSize,
			Expression<Func<TEntity, TDto>> selector,
			CancellationToken ct
		){
			var skip = (pageIndex - 1) * pageSize;
			var pageQuery = ordered.Skip(skip).Take(pageSize);

			var items = await pageQuery.Select(selector).ToListAsync(ct);
			var total = await ordered.CountAsync(ct); // ordered vẫn là IQueryable, OK

			return new PagedResult<TDto>(items, total, pageIndex, pageSize);
		}
	}
}
