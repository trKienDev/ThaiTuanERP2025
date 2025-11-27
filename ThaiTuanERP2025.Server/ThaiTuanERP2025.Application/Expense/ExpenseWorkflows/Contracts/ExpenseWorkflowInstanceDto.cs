using ThaiTuanERP2025.Domain.Shared.Enums;

namespace ThaiTuanERP2025.Application.Expense.ExpenseWorkflows.Contracts
{
	public sealed record ExpenseWorkflowInstanceDto
	{
		public Guid Id { get; init; }
		public DocumentType DocumentType { get; init; }
		public Guid DocumentId { get; init; }
	}
}
