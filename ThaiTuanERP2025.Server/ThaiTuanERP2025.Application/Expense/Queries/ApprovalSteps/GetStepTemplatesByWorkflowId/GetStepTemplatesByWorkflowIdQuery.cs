using MediatR;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Queries.ApprovalSteps.GetStepTemplatesByWorkflowId
{
	public sealed record GetStepTemplatesByWorkflowIdQuery(Guid WorkflowTemplateId) : IRequest<List<ApprovalStepTemplateDto>>
	{
	}
}
