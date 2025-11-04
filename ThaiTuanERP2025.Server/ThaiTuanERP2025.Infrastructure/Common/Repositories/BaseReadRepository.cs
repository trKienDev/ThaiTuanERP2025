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
	public abstract class BaseReadRepository<TEntity, TDto>
		where TEntity : class
		where TDto : class
	{
		protected readonly DbContext _dbContext;
		protected readonly DbSet<TEntity> _dbSet;
		protected readonly IConfigurationProvider _mapperConfig;

		protected BaseReadRepository(DbContext dbContext, IMapper mapper)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			_dbSet = _dbContext.Set<TEntity>();
			if (mapper is null) throw new ArgumentNullException(nameof(mapper));
			_mapperConfig = mapper.ConfigurationProvider;
		}

		protected virtual IQueryable<TEntity> BaseQuery => _dbSet.AsNoTracking();

		public async Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));
			return await _dbSet.AnyAsync(predicate, cancellationToken);
		}

		/// <summary>
		/// Lấy danh sách DTO có thể có filter, keyword, sắp xếp, và phân trang.
		/// </summary>
		public virtual async Task<List<TDto>> GetAllAsync(
			Expression<Func<TEntity, bool>>? filter = null, string? keyword = null, 
			Expression<Func<TEntity, object>>? orderBy = null,
			bool ascending = true, int? page = null, int? pageSize = null,
			CancellationToken cancellationToken = default
		) {
			var query = BaseQuery;

			if (filter != null) query = query.Where(filter);
			if (!string.IsNullOrWhiteSpace(keyword)) query = ApplyKeywordFilter(query, keyword);
			if (orderBy != null) query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

			var projected = query.ProjectTo<TDto>(_mapperConfig);
			if (page.HasValue && pageSize.HasValue)
				projected = projected.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);

			return await projected.ToListAsync(cancellationToken);
		}

		public virtual async Task<List<TDto>> ListAsync (Func<IQueryable<TEntity>, IQueryable<TEntity>> builder, bool asNoTracking = true, CancellationToken cancellationToken = default
		) {
			var query = asNoTracking ? _dbSet.AsNoTracking() : _dbSet.AsQueryable();
			if (builder != null) query = builder(query);
			return await query.ProjectTo<TDto>(_mapperConfig).ToListAsync(cancellationToken);
		}

		/// <summary>
		/// Có thể override để custom logic tìm kiếm theo keyword (tuỳ domain).
		/// </summary>
		protected virtual IQueryable<TEntity> ApplyKeywordFilter(IQueryable<TEntity> query, string keyword)
		{
			return query; // override ở lớp con nếu cần
		}

		public async Task<TDto?> GetByIdProjectedAsync(Guid id, CancellationToken cancellationToken = default)
		{
			return await Query() // AsNoTracking mặc định
				.Where(e => EF.Property<Guid>(e, "Id") == id)
				.ProjectTo<TDto>(_mapperConfig)
				.SingleOrDefaultAsync(cancellationToken);
		}

		public Task<List<TDto>> ListProjectedAsync(Func<IQueryable<TEntity>, IQueryable<TDto>> builder, bool asNoTracking = true, CancellationToken cancellationToken = default)
		{
			var query = asNoTracking ? _dbSet.AsNoTracking() : _dbSet.AsQueryable();
			return builder(query).ToListAsync(cancellationToken); // materialize trong repo
		}

		public IQueryable<TEntity> Query(bool asNoTracking = true)
		{
			return asNoTracking ? _dbSet.AsNoTracking() : _dbSet.AsQueryable();
		}
	}
}