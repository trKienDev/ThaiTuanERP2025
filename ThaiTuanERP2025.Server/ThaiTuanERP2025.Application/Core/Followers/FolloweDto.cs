using ThaiTuanERP2025.Domain.Core.Enums;
using ThaiTuanERP2025.Domain.Shared.Enums;

namespace ThaiTuanERP2025.Application.Core.Followers
{
	public sealed record FolloweDto
	{
		public Guid DocumentId { get; init; }
		public DocumentType DocumentType { get; init; }
		public Guid UserId { get; init; }
		public bool IsActive { get; private set; } = true;
	}
}
