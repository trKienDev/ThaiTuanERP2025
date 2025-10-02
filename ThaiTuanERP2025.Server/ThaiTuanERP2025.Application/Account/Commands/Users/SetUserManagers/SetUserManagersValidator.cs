using FluentValidation;

namespace ThaiTuanERP2025.Application.Account.Commands.Users.SetUserManagers
{
	public sealed class SetUserManagersValidator : AbstractValidator<SetUserManagersCommand>
	{
		public SetUserManagersValidator()
		{
			RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId không được để trống");
			RuleFor(x => x.ManagerIds).NotNull().WithMessage("ManagerIds không được để trống");

			RuleFor(x => x.ManagerIds)
				.Must(managerIds => managerIds.Distinct().Count() == managerIds.Count)
				.WithMessage("ManagerIds không được chứa các giá trị trùng lặp");

			RuleFor(x => x)
				.Must(x => !x.ManagerIds.Contains(x.UserId))
				.WithMessage("Không thể tự làm quản lý của chính mình");

			RuleFor(x => x.PrimaryManagerId)
				.Must((command, primaryManagerId) =>
				{
					// Nếu PrimaryManagerId không null thì phải nằm trong danh sách ManagerIds
					return primaryManagerId == null || command.ManagerIds.Contains(primaryManagerId.Value);
				})
				.WithMessage("PrimaryManagerId phải nằm trong danh sách ManagerIds nếu được cung cấp");
		}
	}
}
