using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common.Events;

namespace ThaiTuanERP2025.Domain.Account.Events.Departments
{
	public sealed class DepartmentDeactivatedEvent : IDomainEvent
	{
		public DepartmentDeactivatedEvent(Department department)
		{
			Department = department;
			OccurredOn = DateTime.UtcNow;
		}

		public Department Department { get; }
		public DateTime OccurredOn { get; }
	}
}
