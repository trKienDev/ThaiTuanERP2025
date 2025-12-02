using ThaiTuanERP2025.Domain.Shared.Repositories;
using ThaiTuanERP2025.Domain.Core.Entities;

namespace ThaiTuanERP2025.Domain.Core.Repositories
{
	public interface IStoredFilesRepository : IBaseWriteRepository<StoredFile>
	{
		Task<List<StoredFile>> ListByEntitiesAsync(string module, string entity, string entityId, CancellationToken cancellationToken);
		Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken);
		Task<bool> HardDeleteAsync(Guid id, CancellationToken cancellationToken);
	}
}
