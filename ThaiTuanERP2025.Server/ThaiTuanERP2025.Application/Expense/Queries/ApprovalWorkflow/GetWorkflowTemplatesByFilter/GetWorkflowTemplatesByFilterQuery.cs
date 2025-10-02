using MediatR;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Queries.ApprovalWorkflow.GetWorkflowTemplatesByFilter
{
	public sealed record GetWorkflowTemplatesByFilterQuery(
		string? DocumentType,
		bool? IsActive
	) : IRequest<IReadOnlyList<ApprovalWorkflowTemplateDto>>;
}
