using FluentValidation;
using ThaiTuanERP2025.Application.Expense.Requests;

namespace ThaiTuanERP2025.Application.Expense.Validators
{
	public class CreateApprovalWorkflowRequestValidator : AbstractValidator<CreateApprovalWorkflowRequest>
	{
		public CreateApprovalWorkflowRequestValidator() {
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Tên không được để trống")
				.MaximumLength(256).WithMessage("Tên không được vượt quá 256 ký tự");
			RuleFor(x => x.Steps).NotNull().NotEmpty();
			RuleForEach(x => x.Steps).ChildRules(steps =>
			{
				steps.RuleFor(x => x.Title).NotEmpty().MaximumLength(256);
				steps.RuleFor(x => x.Order).GreaterThan(0);
				steps.RuleFor(x => x.CandidateUserIds).NotNull().Must(l => l.Count > 0)
					.WithMessage("Mỗi bước duyệt phải có ít nhất 1 người duyệt");
				steps.RuleFor(x => x.SlaHours).GreaterThanOrEqualTo(0).When(s => s.SlaHours.HasValue);
			});

			// Order trong 1 workflow phải là unique
			RuleFor(x => x)
				.Must(x => x.Steps.Select(s => s.Order).Distinct().Count() == x.Steps.Count)
				.WithMessage("Order của các bước duyệt phải là duy nhất");
		}
	}
}
