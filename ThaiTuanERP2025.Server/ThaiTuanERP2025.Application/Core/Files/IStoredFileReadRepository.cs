using ThaiTuanERP2025.Domain.Shared.Repositories;
using ThaiTuanERP2025.Domain.Core.Entities;

namespace ThaiTuanERP2025.Application.Core.Files
{
	public interface IStoredFileReadRepository : IBaseWriteRepository<StoredFile>
	{
		Task<Dictionary<Guid, string>> GetObjectKeysByIdsAsync(IEnumerable<Guid> fileIds, CancellationToken cancellationToken = default);
	}
}
