using FluentValidation;

namespace ThaiTuanERP2025.Application.Account.Commands.RBAC.AssignRoleToUser
{
	public sealed class AssignRoleToUserValidator : AbstractValidator<AssignRoleToUserCommand>
	{
		public AssignRoleToUserValidator() {
			RuleFor(x => x.UserId).NotEmpty();
			RuleFor(x => x.RoleId).NotEmpty();
		}
	}
}
