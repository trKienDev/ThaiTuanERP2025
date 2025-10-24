using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Files.Entities;

namespace ThaiTuanERP2025.Application.Files.Repositories
{
	public interface IStoredFilesRepository : IBaseRepository<StoredFile>
	{
		Task<List<StoredFile>> ListByEntitiesAsync(string module, string entity, string entityId, CancellationToken cancellationToken);
		Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken);
		Task<bool> HardDeleteAsync(Guid id, CancellationToken cancellationToken);
	}
}
