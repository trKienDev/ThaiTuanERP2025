using MediatR;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalWorkflows.CreateApprovalWorkflowTemplate
{
	public sealed record CreateApprovalWorkflowTemplateCommand(ApprovalWorkflowTemplateRequest Request) : IRequest<ApprovalWorkflowTemplateDto>;
}
