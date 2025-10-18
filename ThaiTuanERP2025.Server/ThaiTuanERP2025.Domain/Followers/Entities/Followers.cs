using System.ComponentModel.DataAnnotations.Schema;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Followers.Enums;

namespace ThaiTuanERP2025.Domain.Followers.Entities
{
	public class Follower : AuditableEntity
	{
		private Follower() { }

		public Follower(SubjectType subjectType, Guid subjectId, Guid userId)
		{
			Id = Guid.NewGuid();
			SubjectType = subjectType;
			SubjectId = subjectId;
			UserId = userId;
		}

		public SubjectType SubjectType { get; private set; } = default!;
		public Guid SubjectId { get; private set; }
		public Guid UserId { get; private set; }

		// (tuỳ chọn) tiện đọc code, không map
		[NotMapped]
		public SubjectRef Subject => new(SubjectType, SubjectId);
	}
}
