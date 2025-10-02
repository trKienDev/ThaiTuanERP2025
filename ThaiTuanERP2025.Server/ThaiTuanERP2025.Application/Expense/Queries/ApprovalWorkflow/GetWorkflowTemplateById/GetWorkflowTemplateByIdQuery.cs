using MediatR;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Queries.ApprovalWorkflow.GetWorkflowTemplateById
{
	public sealed record GetWorkflowTemplateByIdQuery(Guid Id) : IRequest<ApprovalWorkflowTemplateDto?>;
}
