using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Domain.Core.Repositories;

namespace ThaiTuanERP2025.Infrastructure.Core.Repositories
{
	public class StoredFilesRepository : BaseWriteRepository<StoredFile>, IStoredFilesRepository
	{
		private readonly ThaiTuanERP2025DbContext _dbContext;
		public StoredFilesRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider configurationProvider) : base(dbContext, configurationProvider)
		{
			_dbContext = dbContext;
		}

		public async Task<List<StoredFile>> ListByEntitiesAsync(string module, string entity, string entityId, CancellationToken cancellationToken)
		{
			return await _dbContext.StoredFiles.Where(x => x.Module == module && x.Entity == entity && x.EntityId == entityId)
				.OrderByDescending(x => x.CreatedAt).ToListAsync();
		}

		public async Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken) {
			// lấy entity theo filter mặc định (!IsDeleted nếu có query filter)
			var entity = await _dbContext.StoredFiles.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
			if (entity == null) return false;

			// dùng Delete() của BaseRepository => sẽ xóa mềm vì StoredFile kế thừa AuditableEntity
			Delete(entity); // đặt IsDeleted=true, DeletedDate=UtcNow và Update entity
			return true;
		}

		public async Task<bool> HardDeleteAsync(Guid id, CancellationToken cancellationToken) {
			// bỏ qua query filter để vẫn tìm được bản ghi đã bị soft-delete
			var entity = await _dbContext.StoredFiles.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
			if (entity == null) 
				return false;

			_dbContext.StoredFiles.Remove(entity);
			return true;
		}
	}
}
