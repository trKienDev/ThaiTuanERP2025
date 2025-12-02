using ThaiTuanERP2025.Domain.StoredFiles.Entities;

namespace ThaiTuanERP2025.Application.Files.Authorization.Interfaces
{
	public interface IStoredFilePermissionChecker
	{
		bool CanHandle(string module, string entity);
		Task<bool> HasPermissionAsync(StoredFile file, Guid userId, CancellationToken cancellationToken);
	}
}
