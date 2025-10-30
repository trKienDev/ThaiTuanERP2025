using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common.Events;

namespace ThaiTuanERP2025.Domain.Account.Events.Departments
{
	public sealed class DepartmentRenamedEvent : IDomainEvent
	{
		public DepartmentRenamedEvent(Department department)
		{
			Department = department;
			OccurredOn = DateTime.UtcNow;
		}

		public Department Department { get; }
		public DateTime OccurredOn { get; }
	}
}
