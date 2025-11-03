using FluentValidation;

namespace ThaiTuanERP2025.Application.Account.Roles.Commands.CreateRole
{
	public sealed class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
	{
		public CreateRoleCommandValidator()
		{
			RuleFor(x => x.Request.Name).NotEmpty().WithMessage("Role name is required.")
				.MaximumLength(100);

			RuleFor(x => x.Request.Description).MaximumLength(255).WithMessage("Mô tả không vượt quá 255 ký tự");
		}
	}
}
