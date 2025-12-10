using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Application.Core.Followers;
using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Domain.Shared.Enums;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;

namespace ThaiTuanERP2025.Infrastructure.Core.Repositories.Read
{
	public sealed class FollowerReadRepository : BaseReadRepository<Follower, FolloweDto>, IFollowerReadRepository
	{
		public FollowerReadRepository(ThaiTuanERP2025DbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

		public async Task<IReadOnlyList<Guid>> GetFollowerIdsByDocument(Guid documentId, DocumentType documentType, CancellationToken cancellationToken)
		{
			return await _dbSet
				.Where(x => x.DocumentId == documentId && x.DocumentType == documentType)
				.Select(x => x.UserId)	
				.ToListAsync();
		}

		public async Task<IReadOnlyList<Guid>> GetFollowingDocumentIdsByType(Guid userId, DocumentType documentType, CancellationToken cancellationToken)
		{
			return await _dbSet
				.Where(x => x.UserId == userId && x.DocumentType == documentType)
				.Select(x => x.DocumentId)
				.ToListAsync();
		}
	}
}
