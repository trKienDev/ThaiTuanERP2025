using Drive.Domain.Shared.Repositories;
using ThaiTuanERP2025.Domain.StoredFiles.Entities;

namespace Drive.Domain.Repositories
{
	public interface IStoredObjectWriteRepository : IBaseWriteRepository<StoredObject>
	{
		Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken);
		Task<bool> HardDeleteAsync(Guid id, CancellationToken cancellationToken);
	}
}
