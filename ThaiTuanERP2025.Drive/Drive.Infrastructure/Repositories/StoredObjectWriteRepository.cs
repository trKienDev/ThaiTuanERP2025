using AutoMapper;
using Drive.Domain.Repositories;
using Drive.Infrastructure.Persistence;
using Drive.Infrastructure.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Domain.StoredFiles.Entities;

namespace Drive.Infrastructure.Repositories
{
	public class StoredObjectWriteRepository : BaseWriteRepository<StoredObject>, IStoredObjectWriteRepository
	{
		private readonly ThaiTuanERP2025DriveDbContext _dbContext;
		public StoredObjectWriteRepository(ThaiTuanERP2025DriveDbContext dbContext, IConfigurationProvider mapperConfig) : base(dbContext, mapperConfig)
		{
			_dbContext = dbContext;
		}

		public async Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken)
		{
			// lấy entity theo filter mặc định (!IsDeleted nếu có query filter)
			var entity = await _dbContext.StoredFiles.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
			if (entity == null) return false;

			// dùng Delete() của BaseRepository => sẽ xóa mềm vì StoredFile kế thừa AuditableEntity
			Delete(entity); // đặt IsDeleted=true, DeletedDate=UtcNow và Update entity
			return true;
		}

		public async Task<bool> HardDeleteAsync(Guid id, CancellationToken cancellationToken)
		{
			// bỏ qua query filter để vẫn tìm được bản ghi đã bị soft-delete
			var entity = await _dbContext.StoredFiles.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
			if (entity == null)
				return false;

			_dbContext.StoredFiles.Remove(entity);
			return true;
		}
	}
}
