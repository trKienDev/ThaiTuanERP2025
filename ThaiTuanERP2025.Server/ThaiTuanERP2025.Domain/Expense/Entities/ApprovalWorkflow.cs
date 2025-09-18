using ThaiTuanERP2025.Domain.Common;

namespace ThaiTuanERP2025.Domain.Expense.Entities
{
	public class ApprovalWorkflow : AuditableEntity
	{
		public string Name { get; private set; } = string.Empty;
		
		private readonly List<ApprovalStep> _steps = new List<ApprovalStep>();
		public IReadOnlyList<ApprovalStep> Steps => _steps;

		private ApprovalWorkflow() { }

		public ApprovalWorkflow(string name, List<ApprovalStep> steps)
		{
			Name = name;
			_steps = steps ?? new List<ApprovalStep>();
		}

		public void AddStep(ApprovalStep step) => _steps.Add(step);
	}
}
