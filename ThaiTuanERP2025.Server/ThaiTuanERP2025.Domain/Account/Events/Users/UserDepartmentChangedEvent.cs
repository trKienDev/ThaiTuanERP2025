using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common.Events;

namespace ThaiTuanERP2025.Domain.Account.Events.Users
{
	public sealed class UserDepartmentChangedEvent : IDomainEvent
	{
		public UserDepartmentChangedEvent(User user, Guid departmentId)
		{
			User = user;
			DepartmentId = departmentId;
			OccurredOn = DateTime.UtcNow;
		}

		public User User { get; }
		public Guid DepartmentId { get; }
		public DateTime OccurredOn { get; }
	}
}
