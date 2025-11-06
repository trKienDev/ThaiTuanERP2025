using FluentValidation;

namespace ThaiTuanERP2025.Application.Finance.BudgetGroups.Commands.Create
{
	public sealed class CreateBudgetGroupCommandValidator : AbstractValidator<CreateBudgetGroupCommand>
	{
		public CreateBudgetGroupCommandValidator() {
			RuleFor(x => x.Name)
				.Cascade(CascadeMode.Stop)
				.NotEmpty().WithMessage("Tên nhóm ngân sách (Name) là bắt buộc.")
				.Must(v => !string.IsNullOrWhiteSpace(v))
				.WithMessage("Tên nhóm ngân sách không được chỉ toàn khoảng trắng.")
				.Must(v => v!.Trim().Length <= 200)
				.WithMessage("Tên nhóm ngân sách không được vượt quá 200 ký tự.");

			RuleFor(x => x.Code)
				.Cascade(CascadeMode.Stop)
				.NotEmpty().WithMessage("Mã nhóm ngân sách (Code) là bắt buộc.")
				.Must(v => !string.IsNullOrWhiteSpace(v))
				.WithMessage("Mã nhóm ngân sách không được chỉ toàn khoảng trắng.")
				.Must(v => v!.Trim().Length <= 50)
				.WithMessage("Mã nhóm ngân sách không được vượt quá 50 ký tự.");
		}
 	}
}
