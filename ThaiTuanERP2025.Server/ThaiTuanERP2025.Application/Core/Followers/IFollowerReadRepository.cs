using ThaiTuanERP2025.Application.Shared.Repositories;
using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Domain.Shared.Enums;

namespace ThaiTuanERP2025.Application.Core.Followers
{
	public interface IFollowerReadRepository : IBaseReadRepository<Follower, FolloweDto>
	{
		Task<IReadOnlyList<Guid>> GetFollowerIdsByDocument(Guid documentId, DocumentType documentType, CancellationToken cancellationToken);
		Task<IReadOnlyList<Guid>> GetFollowingDocumentIdsByType(Guid userId, DocumentType documentType, CancellationToken cancellationToken);
	}
}
