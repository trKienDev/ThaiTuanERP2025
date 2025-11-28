using ThaiTuanERP2025.Domain.Core.Enums;
using ThaiTuanERP2025.Domain.Shared.Enums;

namespace ThaiTuanERP2025.Application.Core.Followers
{
	public interface IFollowerService
	{
		Task FollowAsync(Guid documentId, DocumentType documentType, Guid userId, CancellationToken cancellationToken);
		Task FollowManyAsync(DocumentType subjectType, Guid subjectId, IEnumerable<Guid> userIds, CancellationToken cancellationToken);
		Task UnfollowAsync(DocumentType documentType, Guid documentId, Guid userId, CancellationToken cancellationToken);
		Task<bool> IsFollowingAsync(DocumentType documentType, Guid documentId, Guid userId, CancellationToken cancellationToken);
	}
}
