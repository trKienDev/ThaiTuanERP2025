using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Domain.StoredFiles.Repositories
{
	public interface IFileAttachmentWriteRepository : IBaseWriteRepository<FileAttachment>
	{
		Task<List<FileAttachment>> ListByEntitiesAsync(string module, string entity, string entityId, CancellationToken cancellationToken);
		Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken);
		Task<bool> HardDeleteAsync(Guid id, CancellationToken cancellationToken);
	}
}
