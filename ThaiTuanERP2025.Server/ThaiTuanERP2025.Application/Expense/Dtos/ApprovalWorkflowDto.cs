using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Application.Expense.Dtos
{
	public class ApprovalStepDto
	{
		public Guid Id { get; set; }
		public string Title { get; set; } = default!;
		public int Order { get; set; }
		public ApprovalStepFlowType FlowType { get; set; }
		public int SlaHours { get; set; }
		public string[] CandidateUserIds { get; set; } = Array.Empty<string>();
	}

	public class ApprovalWorkflowDto {
		public Guid Id { get; set; }
		public string Name { get; set; } = default!;
		public bool IsActive { get; set; }
		public List<ApprovalStepDto> Steps { get; set; } = new();
	}
}
