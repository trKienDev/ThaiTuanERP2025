using MediatR;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Application.Expense.Requests;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalWorkflows.UpdateApprovalWorkflow
{
	public sealed record UpdateApprovalWorkflowCommand(Guid Id, UpdateApprovalWorkflowRequest Request) : IRequest<ApprovalWorkflowDto?>;
}
