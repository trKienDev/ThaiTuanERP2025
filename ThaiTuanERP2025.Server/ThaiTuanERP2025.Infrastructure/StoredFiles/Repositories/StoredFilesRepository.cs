using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Account.Repositories;
using ThaiTuanERP2025.Application.Files.Repositories;
using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Files.Entities;
using ThaiTuanERP2025.Infrastructure.Common;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.StoredFiles.Repositories
{
	public class StoredFilesRepository : BaseRepository<StoredFile>, IStoredFilesRepository
	{
		private readonly ThaiTuanERP2025DbContext _dbContext;
		public StoredFilesRepository(ThaiTuanERP2025DbContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}

		public async Task<List<StoredFile>> ListByEntitiesAsync(string module, string entity, string entityId, CancellationToken cancellationToken)
		{
			return await _dbContext.StoredFiles.Where(x => x.Module == module && x.Entity == entity && x.EntityId == entityId)
				.OrderByDescending(x => x.CreatedDate).ToListAsync();
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
