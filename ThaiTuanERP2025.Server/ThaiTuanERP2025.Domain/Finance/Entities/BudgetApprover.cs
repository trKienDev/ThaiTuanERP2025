using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Shared;
using ThaiTuanERP2025.Domain.Shared.Entities;
using ThaiTuanERP2025.Domain.Finance.Events;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class BudgetApprover : AuditableEntity
	{
		private readonly List<BudgetApproverDepartment> _departments = new();

		#region Properties
		public Guid ApproverUserId { get; private set; }
		public User ApproverUser { get; init; } = null!;
		public int SlaHours { get; private set; } = 8; 
		public bool IsActive { get; private set; } = true;

		public IReadOnlyCollection<BudgetApproverDepartment> Departments => _departments.AsReadOnly();
		#endregion

		#region EF Constructor
		private BudgetApprover() { }
		public BudgetApprover(Guid approverId, int slaHours) {
			Guard.AgainstDefault(approverId, nameof(approverId));
			Guard.AgainstNullOrEmptyGuid(approverId, nameof(approverId));
			Guard.AgainstNegativeOrZero(slaHours, nameof(slaHours));

			ApproverUserId = approverId;
			SlaHours = slaHours;
			IsActive = true;
		}
		#endregion

		#region Domain Behaviors
		internal void UpdateApprover(Guid approverId, int slaHours)
		{
			Guard.AgainstDefault(approverId, nameof(approverId));
			Guard.AgainstNullOrEmptyGuid(approverId, nameof(approverId));
			Guard.AgainstNegativeOrZero(slaHours, nameof(slaHours));

			ApproverUserId = approverId;
			SlaHours = slaHours;
			AddDomainEvent(new BudgetApproverChangedEvent(Id, approverId, slaHours));
		}

		internal void Activated() => IsActive = true;
		internal void Deactive() => IsActive = false;

		internal void AssignToDepartment(Guid departmentId)
		{
			Guard.AgainstDefault(departmentId, nameof(departmentId));

			if (_departments.Any(d => d.DepartmentId == departmentId)) return;

			_departments.Add(new BudgetApproverDepartment(Id, departmentId));
			// AddDomainEvent(new BudgetApproverDepartmentAssignedEvent(Id, departmentId));
		}

		internal void RemoveFromDepartment(Guid departmentId) {
			var link = _departments.FirstOrDefault(d => d.DepartmentId == departmentId);
			if (link == null) return;

			_departments.Remove(link);
			// AddDomainEvent(new BudgetApproverDepartmentRemovedEvent(Id, departmentId));
		}

		#endregion

	}
}
