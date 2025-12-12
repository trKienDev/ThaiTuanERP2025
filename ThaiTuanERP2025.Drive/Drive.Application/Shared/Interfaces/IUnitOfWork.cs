using Drive.Domain.Repositories;

namespace Drive.Application.Shared.Interfaces
{
	public interface IUnitOfWork
	{
		IStoredObjectWriteRepository StoredFiles { get; }
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}
