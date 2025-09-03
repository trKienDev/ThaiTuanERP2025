using MediatR;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Application.Expense.Requests;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalWorkflows.CreateApprovalWorkflow
{
	public sealed record CreateApprovalWorkflowCommand(CreateApprovalWorkflowRequest Request) : IRequest<ApprovalWorkflowDto>;
}
