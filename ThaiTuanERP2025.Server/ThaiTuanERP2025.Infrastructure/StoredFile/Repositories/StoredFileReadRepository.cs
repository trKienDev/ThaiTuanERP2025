using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Application.Files;
using ThaiTuanERP2025.Domain.StoredFiles.Entities;

namespace ThaiTuanERP2025.Infrastructure.StoredFiles.Repositories
{
	public sealed class StoredFileReadRepository : BaseWriteRepository<StoredFile>, IStoredFileReadRepository
	{
		private ThaiTuanERP2025DbContext DbContext => (ThaiTuanERP2025DbContext)_context;
		public StoredFileReadRepository(ThaiTuanERP2025DbContext context, IConfigurationProvider configurationProvider)
			: base(context, configurationProvider) { }


		public async Task<Dictionary<Guid, string>> GetObjectKeysByIdsAsync(IEnumerable<Guid> fileIds, CancellationToken cancellationToken = default)
		{
			return await _dbSet.AsNoTracking()
				.Where(f => fileIds.Contains(f.Id))
				.ToDictionaryAsync(f => f.Id, f => f.ObjectKey, cancellationToken);
		}
	}
}
