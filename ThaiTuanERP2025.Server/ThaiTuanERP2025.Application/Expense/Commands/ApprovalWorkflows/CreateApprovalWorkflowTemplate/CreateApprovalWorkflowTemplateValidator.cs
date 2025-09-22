using FluentValidation;
using ThaiTuanERP2025.Application.Expense.Dtos;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalWorkflows.CreateApprovalWorkflowTemplate
{
	public sealed class CreateApprovalWorkflowTemplateValidator : AbstractValidator<ApprovalWorkflowTemplateRequest>
	{
		public CreateApprovalWorkflowTemplateValidator() { 
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Tên luồng duyệt không được để trống")
				.MaximumLength(200).WithMessage("Tên luồng duyệt không được vượt quá 200 ký tự");

			RuleFor(x => x.DocumentType)
				.NotEmpty().WithMessage("Loại thanh toán không được để trống");
		}
	}
}
