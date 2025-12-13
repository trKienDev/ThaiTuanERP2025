using ThaiTuanERP2025.Application.Drive.Contracts;

namespace ThaiTuanERP2025.Application.Drive.Intergrations
{
	public interface IDriveQueryClient
	{
		Task<IReadOnlyList<DriveObjectDto>> GetObjectsByIdsAsync(IReadOnlyList<Guid> ids, CancellationToken cancellationToken);
	}
}
