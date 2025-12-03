using ThaiTuanERP2025.Domain.Shared.Repositories;
using ThaiTuanERP2025.Domain.StoredFiles.Entities;

namespace ThaiTuanERP2025.Application.Files
{
	public interface IStoredFileReadRepository : IBaseWriteRepository<StoredFile>
	{
		Task<Dictionary<Guid, string>> GetObjectKeysByIdsAsync(IEnumerable<Guid> fileIds, CancellationToken cancellationToken = default);
	}
}
