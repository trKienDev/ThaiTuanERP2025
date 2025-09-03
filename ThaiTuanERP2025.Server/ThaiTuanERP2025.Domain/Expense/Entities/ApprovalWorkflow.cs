using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ApprovalWorkflow : AuditableEntity
	{
		public string Name { get; set; } = default!;
		public bool IsActive { get; set; } = true;

		private readonly List<ApprovalStep> _steps = new();
		public ICollection<ApprovalStep> Steps => _steps;

		public void AddStep(ApprovalStep step) {
			if(step is null) 
				throw new ArgumentNullException(nameof(step));
			_steps.Add(step);
		}
	}
}
