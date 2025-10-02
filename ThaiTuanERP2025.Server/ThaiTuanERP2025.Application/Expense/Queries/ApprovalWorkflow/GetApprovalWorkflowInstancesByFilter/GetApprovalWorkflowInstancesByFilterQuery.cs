using MediatR;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Application.Expense.Queries.ApprovalWorkflow.GetApprovalWorkflowInstancesByFilter
{
	public sealed record GetApprovalWorkflowInstancesByFilterQuery(
		string? DocumentType,
		Guid? DocumentId,
		WorkflowStatus? Status,
		string? BudgetCode,
		decimal? MinAmount,
		decimal? MaxAmount
	) : IRequest<IReadOnlyList<ApprovalWorkflowInstanceDto>>;
}
