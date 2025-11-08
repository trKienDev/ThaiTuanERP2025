using ThaiTuanERP2025.Domain.Account.Entities;
using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Finance.Entities
{
	public class BudgetApproverDepartment	{
		#region Properties
		public Guid BudgetApproverId { get; private set; } = Guid.Empty!;
		public BudgetApprover BudgetApprover { get; private set; } = null!;

		public Guid DepartmentId { get; private set; } = Guid.Empty!;
		public Department Department { get; private set; } = null!;
		#endregion

		#region 
		private BudgetApproverDepartment() { }
		public BudgetApproverDepartment(Guid budgetApproverId, Guid departmentId)
		{
			Guard.AgainstDefault(budgetApproverId, nameof(budgetApproverId));
			Guard.AgainstDefault(departmentId, nameof(departmentId));

			BudgetApproverId = budgetApproverId;
			DepartmentId = departmentId;
		}
		#endregion
	}
}
