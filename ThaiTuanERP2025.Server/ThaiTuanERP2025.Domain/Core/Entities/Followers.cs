using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Entities;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Core.Enums;
using ThaiTuanERP2025.Domain.Shared.Interfaces;

namespace ThaiTuanERP2025.Domain.Core.Entities
{
	public class Follower : AuditableEntity, IActiveEntity
	{
		#region Constructors
		private Follower() { } 
		public Follower(Guid subjectId, SubjectType subjectType, Guid userId)
		{
			Guard.AgainstDefault(userId, nameof(userId));

			Id = Guid.NewGuid();
			SubjectId = subjectId;
			SubjectType = subjectType;
			UserId = userId;
			IsActive = true;

			// AddDomainEvent(new FollowerCreatedEvent(this));
		}
		#endregion

		#region Properties
		public Guid SubjectId { get; private set; }
		public SubjectType SubjectType { get; private set; }
		public Guid UserId { get; private set; }
		public bool IsActive { get; private set; } = true;
		#endregion

		#region Domain Behaviors
		internal void Unfollow()
		{
			if (!IsActive)
				throw new DomainException("Follower đã bị huỷ trước đó.");

			IsActive = false;
			// AddDomainEvent(new FollowerRemovedEvent(this));
		}

		internal void Reactivate()
		{
			if (IsActive)
				return;

			IsActive = true;
			// AddDomainEvent(new FollowerReactivatedEvent(this));
		}
		#endregion
	}
}
