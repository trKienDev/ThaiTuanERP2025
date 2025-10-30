using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common.Events;

namespace ThaiTuanERP2025.Domain.Account.Events.Departments
{

	public sealed class DepartmentChildRemovedEvent : IDomainEvent
	{
		public DepartmentChildRemovedEvent(Department parent, Department child)
		{
			Parent = parent;
			Child = child;
			OccurredOn = DateTime.UtcNow;
		}

		public Department Parent { get; }
		public Department Child { get; }
		public DateTime OccurredOn { get; }
	}
}
