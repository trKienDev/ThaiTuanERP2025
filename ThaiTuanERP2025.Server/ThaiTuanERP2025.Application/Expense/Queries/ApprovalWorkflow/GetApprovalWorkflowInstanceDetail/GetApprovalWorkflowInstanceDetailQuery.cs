using MediatR;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Queries.ApprovalWorkflow.GetApprovalWorkflowInstanceDetail
{
	public sealed record GetApprovalWorkflowInstanceDetailQuery(Guid Id) : IRequest<ApprovalWorkflowInstanceDetailDto>;
}
