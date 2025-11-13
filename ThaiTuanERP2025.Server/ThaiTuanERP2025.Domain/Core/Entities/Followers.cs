using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Entities;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Followers.ValueObjects;

namespace ThaiTuanERP2025.Domain.Core.Entities
{
	public class Follower : AuditableEntity
	{
		public SubjectRef Subject { get; private set; } = default!;
		public Guid UserId { get; private set; }
		public bool IsActive { get; private set; } = true;

		#region Constructors
		private Follower() { } 
		public Follower(SubjectRef subject, Guid userId)
		{
			Guard.AgainstDefault(userId, nameof(userId));

			Id = Guid.NewGuid();
			Subject = subject;
			UserId = userId;
			IsActive = true;

			// AddDomainEvent(new FollowerCreatedEvent(this));
		}
		#endregion

		#region Domain Behaviors
		public void Unfollow()
		{
			if (!IsActive)
				throw new DomainException("Follower đã bị huỷ trước đó.");

			IsActive = false;
			// AddDomainEvent(new FollowerRemovedEvent(this));
		}

		public void Reactivate()
		{
			if (IsActive)
				return;

			IsActive = true;
			// AddDomainEvent(new FollowerReactivatedEvent(this));
		}
		#endregion
	}
}
