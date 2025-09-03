using MediatR;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Queries.ApprovalWorkflows.GetApprovalWorkflowById
{
	public sealed record GetApprovalWorkflowByIdQuery(Guid Id) : IRequest<ApprovalWorkflowDto>;
}
