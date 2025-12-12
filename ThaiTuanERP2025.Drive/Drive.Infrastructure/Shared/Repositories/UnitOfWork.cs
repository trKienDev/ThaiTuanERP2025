using Drive.Application.Shared.Interfaces;
using Drive.Domain.Repositories;
using Drive.Infrastructure.Persistence;

namespace Drive.Infrastructure.Shared.Repositories
{
	public sealed class UnitOfWork : IUnitOfWork
	{
		private readonly ThaiTuanERP2025DriveDbContext _dbContext;
		public UnitOfWork(
			ThaiTuanERP2025DriveDbContext dbContext,
			IStoredObjectWriteRepository storedFiles
		)
		{
			_dbContext = dbContext;
			StoredFiles = storedFiles;
		}

		public IStoredObjectWriteRepository StoredFiles { get; }

		public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return _dbContext.SaveChangesAsync(cancellationToken);
		}
	}
}
