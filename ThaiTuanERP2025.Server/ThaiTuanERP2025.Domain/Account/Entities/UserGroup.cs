using ThaiTuanERP2025.Domain.Account.Events.UserGroups;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Domain.Account.Entities
{
	public class UserGroup : AuditableEntity
	{
		public Guid UserId { get; private set; }
		public Guid GroupId { get; private set; }

		public User User { get; private set; } = default!;
		public Group Group { get; private set; } = default!;

		public DateTime JoinedAt { get; private set; } = DateTime.UtcNow;
		public DateTime? LeftAt { get; private set; }

		public bool IsActive { get; private set; } = true;

		public User CreatedByUser { get; set; } = null!;
		public User? ModifiedByUser { get; set; }
		public User? DeletedByUser { get; set; }

		private UserGroup() { } // EF only

		public UserGroup(Guid userId, Guid groupId)
		{
			Guard.AgainstDefault(userId, nameof(userId));
			Guard.AgainstDefault(groupId, nameof(groupId));

			Id = Guid.NewGuid();
			UserId = userId;
			GroupId = groupId;
			JoinedAt = DateTime.UtcNow;
			IsActive = true;

			AddDomainEvent(new UserJoinedGroupEvent(userId, groupId));
		}

		#region Domain Behaviors
		public void Leave()
		{
			if (!IsActive)
				throw new DomainException("Người dùng đã rời khỏi nhóm này.");

			IsActive = false;
			LeftAt = DateTime.UtcNow;
			AddDomainEvent(new UserLeftGroupEvent(UserId, GroupId));
		}

		public void Reactivate()
		{
			if (IsActive)
				return;

			IsActive = true;
			LeftAt = null;
			AddDomainEvent(new UserRejoinedGroupEvent(UserId, GroupId));
		}
		#endregion
	}
}
