using ThaiTuanERP2025.Domain.Expense.Enums;
using ThaiTuanERP2025.Domain.Shared.Enums;

namespace ThaiTuanERP2025.Application.Expense.ExpenseWorkflows.Contracts
{
	public sealed record ExpenseWorkflowInstanceDto
	{
		public Guid Id { get; init; }
		public DocumentType DocumentType { get; init; }
		public Guid DocumentId { get; init; }
	}

	public sealed record ExpenseWorkflowInstanceBriefDto {
		public Guid Id { get; init; }
		public ExpenseWorkflowStatus Status { get; init; }
		public int CurrentStepOrder { get; init; }
		public IReadOnlyCollection<ExpenseStepInstanceBriefDto> Steps { get; init; } = Array.Empty<ExpenseStepInstanceBriefDto>();
	}
}
