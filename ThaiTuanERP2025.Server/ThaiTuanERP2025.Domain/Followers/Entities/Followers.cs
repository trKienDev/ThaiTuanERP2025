using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Common.Entities;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Followers.Events;
using ThaiTuanERP2025.Domain.Followers.ValueObjects;

namespace ThaiTuanERP2025.Domain.Followers.Entities
{
	public class Follower : AuditableEntity
	{
		public SubjectRef Subject { get; private set; } = default!;
		public Guid UserId { get; private set; }
		public bool IsActive { get; private set; } = true;

		private Follower() { } // EF only

		public Follower(SubjectRef subject, Guid userId)
		{
			Guard.AgainstDefault(userId, nameof(userId));

			Id = Guid.NewGuid();
			Subject = subject;
			UserId = userId;
			IsActive = true;

			AddDomainEvent(new FollowerCreatedEvent(this));
		}

		#region Domain Behaviors
		public void Unfollow()
		{
			if (!IsActive)
				throw new DomainException("Follower đã bị huỷ trước đó.");

			IsActive = false;
			AddDomainEvent(new FollowerRemovedEvent(this));
		}

		public void Reactivate()
		{
			if (IsActive)
				return;

			IsActive = true;
			AddDomainEvent(new FollowerReactivatedEvent(this));
		}
		#endregion
	}
}
