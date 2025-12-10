using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ThaiTuanERP2025.Infrastructure.Shared.Repositories
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
		protected readonly IMapper _mapper;

		protected BaseReadRepository(DbContext dbContext, IMapper mapper)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			_dbSet = _dbContext.Set<TEntity>();
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_mapperConfig = mapper.ConfigurationProvider;
		}

		protected virtual IQueryable<TEntity> BaseQuery => _dbSet.AsNoTracking();

		public async Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));
			return await _dbSet.AnyAsync(predicate, cancellationToken);
		}

		/// <summary>
		/// Truy vấn danh sách <typeparamref name="TDto"/> với các tính năng lọc, tìm kiếm, sắp xếp
		/// và phân trang tích hợp sẵn.
		///
		/// Đây là phương thức tiêu chuẩn cho các API dạng GetList đơn giản:
		/// - Lọc theo predicate (filter)
		/// - Tìm kiếm theo keyword (override <see cref="ApplyKeywordFilter"/> nếu cần)
		/// - Sắp xếp theo trường bất kỳ
		/// - Phân trang (page, pageSize)
		///
		/// Sử dụng AutoMapper ProjectTo để ánh xạ kết quả sang <typeparamref name="TDto"/>,
		/// đảm bảo hiệu năng và truy vấn được dịch sang SQL tối ưu.
		///
		/// Thích hợp cho các use case CRUD đọc dữ liệu thông thường.
		/// Không phù hợp cho projection phức tạp, join nhiều bảng hoặc kết cấu dữ liệu đặc biệt.
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

		/// <summary>
		/// Truy vấn danh sách <typeparamref name="TDto"/> với khả năng tùy chỉnh query trước khi ProjectTo.
		/// 
		/// Dùng khi cần can thiệp trực tiếp vào <see cref="IQueryable{TEntity}"/>:
		/// - Thêm Include/ThenInclude
		/// - Filter phức tạp không phù hợp với tham số filter của GetAllAsync
		/// - Sắp xếp đặc biệt
		/// - Điều chỉnh AsNoTracking hoặc chuyển sang truy vấn tracking
		///
		/// Vẫn sử dụng AutoMapper ProjectTo để map sang <typeparamref name="TDto"/>.
		/// Không hỗ trợ keyword, sort, pagination tích hợp sẵn.
		/// </summary>
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

		/// <summary>
		/// Truy vấn dữ liệu với projection tùy ý, không phụ thuộc AutoMapper.
		/// 
		/// Dùng khi cần trả về ViewModel hoặc mẫu dữ liệu đặc biệt:
		/// - Projection thủ công bằng Select(...)
		/// - Join nhiều bảng, GroupBy, Sum, Count, biểu thức phức tạp
		/// - Lấy cấu trúc tree hoặc flatten records
		/// - Tối ưu hóa truy vấn chỉ lấy đúng trường cần dùng
		///
		/// Thích hợp cho báo cáo, thống kê, dashboard hoặc các payload chỉ dùng cho đọc.
		/// Không sử dụng AutoMapper ProjectTo. Kết quả phụ thuộc hoàn toàn vào builder.
		/// </summary>
		public Task<List<TProjection>> ListProjectedAsync<TProjection>(
			Func<IQueryable<TEntity>, IQueryable<TProjection>> builder, bool asNoTracking = true, CancellationToken cancellationToken = default
		) {
			var query = asNoTracking ? _dbSet.AsNoTracking() : _dbSet.AsQueryable();
			return builder(query).ToListAsync(cancellationToken);
		}

		public IQueryable<TEntity> Query(bool asNoTracking = true)
		{
			return asNoTracking ? _dbSet.AsNoTracking() : _dbSet.AsQueryable();
		}

		public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));
			return _dbSet.AsNoTracking().CountAsync(predicate, cancellationToken);
		}
	}
}