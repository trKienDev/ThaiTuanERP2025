using MediatR;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Queries.ApprovalWorkflow.GetApprovalWorkflowTemplateDetail
{
	public sealed record GetApprovalWorkflowTemplateDetailQuery(Guid Id) : IRequest<ApprovalWorkflowTemplateDetailDto>;
}
