using ThaiTuanERP2025.Domain.Account.Events;
using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Entities;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Domain.Account.Entities
{
	public class UserManagerAssignment : AuditableEntity
	{
		public Guid UserId { get; private set; }
		public User User { get; private set; } = default!;

		public Guid ManagerId { get; private set; }
		public User Manager { get; private set; } = default!;

		public bool IsPrimary { get; private set; }
		public bool IsActive { get; private set; } = true;

		public DateTime AssignedAt { get; private set; } = DateTime.UtcNow;
		public DateTime? RevokedAt { get; private set; }

		#region EF Core Constructor
		private UserManagerAssignment() { } 
		public UserManagerAssignment(Guid userId, Guid managerId, bool isPrimary)
		{
			Guard.AgainstDefault(userId, nameof(userId));
			Guard.AgainstDefault(managerId, nameof(managerId));

			if (userId == managerId)
				throw new DomainException("Không thể tự gán mình làm quản lý.");

			Id = Guid.NewGuid();
			UserId = userId;
			ManagerId = managerId;
			IsPrimary = isPrimary;
			AssignedAt = DateTime.UtcNow;
			IsActive = true;

			AddDomainEvent(new ManagerAssignedToUserEvent(this));
		}
		#endregion

		#region Domain Behaviors
		public void PromoteToPrimary()
		{
			if (IsPrimary) return;
			IsPrimary = true;
			AddDomainEvent(new ManagerPromotedToPrimaryEvent(this));
		}

		public void DemoteFromPrimary()
		{
			if (!IsPrimary) return;
			IsPrimary = false;
			AddDomainEvent(new ManagerDemotedFromPrimaryEvent(this));
		}

		public void Revoke()
		{
			if (!IsActive)
				throw new DomainException("Quan hệ quản lý đã bị thu hồi trước đó.");

			IsActive = false;
			RevokedAt = DateTime.UtcNow;
			AddDomainEvent(new ManagerAssignmentRevokedEvent(this));
		}

		public void Reactivate()
		{
			if (IsActive)
				return;

			IsActive = true;
			RevokedAt = null;
			AddDomainEvent(new ManagerAssignmentReactivatedEvent(this));
		}
		#endregion
	}
}
