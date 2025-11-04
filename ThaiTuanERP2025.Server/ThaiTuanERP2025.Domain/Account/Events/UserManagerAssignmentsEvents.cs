using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common.Events;

namespace ThaiTuanERP2025.Domain.Account.Events
{
	public abstract class UserManagerAssignmentsBaseEvents : IDomainEvent
	{	
		public Guid AssignmentId { get; init; }
		public DateTime OccurredOn { get; }
		protected UserManagerAssignmentsBaseEvents(Guid assignmentId) {
			AssignmentId = assignmentId;
			OccurredOn = DateTime.UtcNow;
		}	
	}

	public sealed class ManagerAssignedToUserEvent : UserManagerAssignmentsBaseEvents
	{
		public ManagerAssignedToUserEvent(UserManagerAssignment assignment) : base(assignment.Id)
		{
			Assignment = assignment;
		}
		public UserManagerAssignment Assignment { get; }
	}

	public sealed class ManagerAssignmentReactivatedEvent : UserManagerAssignmentsBaseEvents
	{
		public ManagerAssignmentReactivatedEvent(UserManagerAssignment assignment) : base(assignment.Id)
		{
			Assignment = assignment;
		}
		public UserManagerAssignment Assignment { get; }
	}

	public sealed class ManagerAssignmentRevokedEvent : UserManagerAssignmentsBaseEvents
	{
		public ManagerAssignmentRevokedEvent(UserManagerAssignment assignment) : base(assignment.Id)
		{
			Assignment = assignment;
		}
		public UserManagerAssignment Assignment { get; }
	}

	public sealed class ManagerDemotedFromPrimaryEvent : UserManagerAssignmentsBaseEvents
	{
		public ManagerDemotedFromPrimaryEvent(UserManagerAssignment assignment) : base(assignment.Id)
		{
			Assignment = assignment;
		}

		public UserManagerAssignment Assignment { get; }
	}

	public sealed class ManagerPromotedToPrimaryEvent : UserManagerAssignmentsBaseEvents
	{
		public ManagerPromotedToPrimaryEvent(UserManagerAssignment assignment) : base(assignment.Id)
		{ 
			Assignment = assignment;
		}
		public UserManagerAssignment Assignment { get; }
	}
}
