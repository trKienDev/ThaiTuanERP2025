using MediatR;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalWorkflows.CreateApprovalWorkflowInstance
{
	public sealed record CreateApprovalWorkflowInstanceCommand(ApprovalWorkflowInstanceRequest Request) : IRequest<ApprovalWorkflowInstanceDto>;
}
