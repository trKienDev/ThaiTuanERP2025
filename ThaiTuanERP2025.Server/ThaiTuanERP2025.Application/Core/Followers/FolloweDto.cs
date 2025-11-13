using ThaiTuanERP2025.Domain.Core.Enums;

namespace ThaiTuanERP2025.Application.Core.Followers
{
	public sealed record FolloweDto
	{
		public Guid SubjectId { get; init; }
		public SubjectType SubjectType { get; init; }
		public Guid UserId { get; init; }
		public bool IsActive { get; private set; } = true;
	}
}
