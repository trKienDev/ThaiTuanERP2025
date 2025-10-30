using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using ThaiTuanERP2025.Application.Common.Models;

namespace ThaiTuanERP2025.Infrastructure.Common.Repositories
{
	/// <summary>
	/// Repository mở rộng ReadRepositoryBase có trả về PagedResult<TDto>.
	/// Phù hợp cho table/grid hoặc API có phân trang.
	/// </summary>
	public abstract class PagedReadRepositoryBase<TEntity, TDto> : ReadRepositoryBase<TEntity, TDto>
		where TEntity : class
		where TDto : class
	{
		protected PagedReadRepositoryBase(DbContext dbContext, IConfigurationProvider mapperConfig) : base(dbContext, mapperConfig) { }

		public virtual async Task<PagedResult<TDto>> GetPagedAsync (
			Expression<Func<TEntity, bool>>? filter = null,
			string? keyword = null,
			Expression<Func<TEntity, object>>? orderBy = null,
			bool ascending = true,
			int page = 1,
			int pageSize = 20,
			CancellationToken cancellationToken = default
		) {
			var query = BaseQuery;

			if (filter != null)
				query = query.Where(filter);

			if (!string.IsNullOrWhiteSpace(keyword))
				query = ApplyKeywordFilter(query, keyword);

			var totalCount = await query.CountAsync(cancellationToken);

			if (orderBy != null)
				query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

			var items = await query
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ProjectTo<TDto>(_mapperConfig)
				.ToListAsync(cancellationToken);

			return new PagedResult<TDto>(items, totalCount, page, pageSize);
		}

		/// <summary>
		/// Kết quả phân trang chuẩn dùng cho API trả về Angular table.
		/// </summary>
		public class PagedResult<T>
		{
			public List<T> Items { get; }
			public int TotalCount { get; }
			public int PageIndex { get; }
			public int PageSize { get; }
			public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

			public bool HasPrevious => PageIndex > 1;
			public bool HasNext => PageIndex < TotalPages;

			public PagedResult(List<T> items, int totalCount, int pageIndex, int pageSize)
			{
				Items = items ?? new List<T>();
				TotalCount = totalCount;
				PageIndex = pageIndex;
				PageSize = pageSize;
			}
		}
	}
}
