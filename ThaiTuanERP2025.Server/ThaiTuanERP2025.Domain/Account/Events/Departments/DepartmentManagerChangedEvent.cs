using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common.Events;

namespace ThaiTuanERP2025.Domain.Account.Events.Departments
{

	public sealed class DepartmentManagerChangedEvent : IDomainEvent
	{
		public DepartmentManagerChangedEvent(Department department, Guid? managerUserId)
		{
			Department = department;
			ManagerUserId = managerUserId;
			OccurredOn = DateTime.UtcNow;
		}

		public Department Department { get; }
		public Guid? ManagerUserId { get; }
		public DateTime OccurredOn { get; }
	}
}
