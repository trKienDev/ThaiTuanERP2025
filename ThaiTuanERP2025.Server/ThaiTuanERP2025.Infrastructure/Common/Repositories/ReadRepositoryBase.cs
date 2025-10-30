using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ThaiTuanERP2025.Infrastructure.Common.Repositories
{
	/// <summary>
	/// Repository cơ bản cho các thao tác đọc dữ liệu (read-only).
	/// Dùng AsNoTracking, hỗ trợ filter, keyword, sort và projection sang DTO.
	/// </summary>
	public abstract class ReadRepositoryBase<TEntity, TDto>
		where TEntity : class
		where TDto : class
	{
		protected readonly DbContext _dbContext;
		protected readonly DbSet<TEntity> _dbSet;
		protected readonly IConfigurationProvider _mapperConfig;

		protected ReadRepositoryBase(DbContext dbContext, IConfigurationProvider mapperConfig)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			_dbSet = _dbContext.Set<TEntity>();
			_mapperConfig = mapperConfig ?? throw new ArgumentNullException(nameof(mapperConfig));
		}

		protected virtual IQueryable<TEntity> BaseQuery => _dbSet.AsNoTracking();

		/// <summary>
		/// Lấy một bản ghi (projected sang DTO) theo Id.
		/// </summary>
		public virtual async Task<TDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
		{
			return await BaseQuery
				.Where(e => EF.Property<Guid>(e, "Id") == id)
				.ProjectTo<TDto>(_mapperConfig)
				.SingleOrDefaultAsync(cancellationToken);
		}

		/// <summary>
		/// Lấy danh sách DTO có thể có filter, keyword, sắp xếp, và phân trang.
		/// </summary>
		public virtual async Task<List<TDto>> GetAllAsync(
			Expression<Func<TEntity, bool>>? filter = null,
			string? keyword = null,
			Expression<Func<TEntity, object>>? orderBy = null,
			bool ascending = true,
			int? page = null,
			int? pageSize = null,
			CancellationToken cancellationToken = default
		) {
			var query = BaseQuery;

			if (filter != null) 
				query = query.Where(filter);

			if (!string.IsNullOrWhiteSpace(keyword))
				query = ApplyKeywordFilter(query, keyword);

			if (orderBy != null)
				query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

			var projected = query.ProjectTo<TDto>(_mapperConfig);

			if (page.HasValue && pageSize.HasValue)
				projected = projected.Skip((page.Value - 1) * pageSize.Value)
					.Take(pageSize.Value);

			return await projected.ToListAsync(cancellationToken);
		}

		/// <summary>
		/// Có thể override để custom logic tìm kiếm theo keyword (tuỳ domain).
		/// </summary>
		protected virtual IQueryable<TEntity> ApplyKeywordFilter(IQueryable<TEntity> query, string keyword)
		{
			return query; // override ở lớp con nếu cần
		}

		/// <summary>
		/// Đếm số lượng bản ghi thoả điều kiện filter.
		/// </summary>
		public virtual async Task<int> CountAsync (
			Expression<Func<TEntity, bool>>? filter = null,
			CancellationToken cancellationToken = default
		) {
			var query = BaseQuery;
			if (filter != null)
				query = query.Where(filter);
			return await query.CountAsync(cancellationToken);
		}

		/// <summary>
		/// Kiểm tra tồn tại bản ghi theo điều kiện filter.
		/// </summary>
		public virtual async Task<bool> ExistsAsync (
			Expression<Func<TEntity, bool>> filter,
			CancellationToken cancellationToken = default
		) {
			return await BaseQuery.AnyAsync(filter, cancellationToken);
		}
	}
}