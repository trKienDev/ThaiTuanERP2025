using ThaiTuanERP2025.Domain.Core.Enums;

namespace ThaiTuanERP2025.Application.Core.Services.Interfaces
{
	public interface IFollowerService
	{
		Task FollowAsync(SubjectType subjectType, Guid subjectId, Guid userId, CancellationToken cancellationToken);
		Task FollowManyAsync(SubjectType subjectType, Guid subjectId, IEnumerable<Guid> userIds, CancellationToken cancellationToken);
		Task UnfollowAsync(SubjectType subjectType, Guid subjectId, Guid userId, CancellationToken cancellationToken);
		Task<bool> IsFollowingAsync(SubjectType subjectType, Guid subjectId, Guid userId, CancellationToken cancellationToken);
	}
}
