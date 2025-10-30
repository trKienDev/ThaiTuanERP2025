using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common.Events;

namespace ThaiTuanERP2025.Domain.Account.Events.UserManagerAssignments
{
	public sealed class ManagerAssignmentReactivatedEvent : IDomainEvent
	{
		public ManagerAssignmentReactivatedEvent(UserManagerAssignment assignment)
		{
			Assignment = assignment;
			OccurredOn = DateTime.UtcNow;
		}

		public UserManagerAssignment Assignment { get; }
		public DateTime OccurredOn { get; }
	}
}
