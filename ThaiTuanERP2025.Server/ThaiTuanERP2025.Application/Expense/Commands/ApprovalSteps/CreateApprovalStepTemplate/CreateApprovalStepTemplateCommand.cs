using MediatR;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalSteps.CreateApprovalStepTemplate
{
	public sealed record CreateApprovalStepTemplateCommand(ApprovalStepTemplateRequest Request) : IRequest<ApprovalStepTemplateDto>;
}
