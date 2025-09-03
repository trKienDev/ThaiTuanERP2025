using MediatR;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Queries.ApprovalWorkflows.GetAllApprovalWorkflow
{
	public sealed record GetAllApprovalWorkflowQuery() : IRequest<IReadOnlyList<ApprovalWorkflowDto>>;
}
