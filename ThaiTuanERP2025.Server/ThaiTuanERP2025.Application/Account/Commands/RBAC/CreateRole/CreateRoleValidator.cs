using FluentValidation;

namespace ThaiTuanERP2025.Application.Account.Commands.RBAC.CreateRole
{
	public sealed class CreateRoleValidator : AbstractValidator<CreateRoleCommand> 
	{
		public CreateRoleValidator() {
			RuleFor(x => x.Request.Name).NotEmpty().WithMessage("Role name is required.")
				.MaximumLength(100);

			RuleFor(x => x.Request.Description).MaximumLength(255);
		}
	}
}
