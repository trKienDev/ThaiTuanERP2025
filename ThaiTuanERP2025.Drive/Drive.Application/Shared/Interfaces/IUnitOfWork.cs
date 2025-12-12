using Drive.Domain.Repositories;

namespace Drive.Application.Shared.Interfaces
{
	public interface IUnitOfWork
	{
		IStoredObjectWriteRepository StoredObjects { get; }
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}
