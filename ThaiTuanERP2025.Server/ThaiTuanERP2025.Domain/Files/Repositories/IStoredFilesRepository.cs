using ThaiTuanERP2025.Domain.Common.Repositories;
using ThaiTuanERP2025.Domain.Files.Entities;

namespace ThaiTuanERP2025.Domain.Files.Repositories
{
	public interface IStoredFilesRepository : IBaseWriteRepository<StoredFile>
	{
		Task<List<StoredFile>> ListByEntitiesAsync(string module, string entity, string entityId, CancellationToken cancellationToken);
		Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken);
		Task<bool> HardDeleteAsync(Guid id, CancellationToken cancellationToken);
	}
}
