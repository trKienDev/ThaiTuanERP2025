using FluentValidation;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Exceptions;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.BudgetGroups.Commands
{
	public sealed record CreateBudgetGroupCommand(string Name, string Code) : IRequest<Unit>;
	public sealed class CreateBudgetGroupCommandHandler : IRequestHandler<CreateBudgetGroupCommand, Unit>
	{
		private readonly IUnitOfWork _unitOfWork;
		public CreateBudgetGroupCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(CreateBudgetGroupCommand command, CancellationToken cancellationToken)
		{
			var nameRaw = command.Name?.Trim() ?? string.Empty;
			Guard.AgainstNullOrWhiteSpace(nameRaw, nameof(command.Name));

			var codeRaw = command.Code?.Trim() ?? string.Empty;
			Guard.AgainstNullOrWhiteSpace(codeRaw, nameof(command.Code));

			var codeNorm = codeRaw.ToUpperInvariant();
			var nameNorm = nameRaw;

			var existed = await _unitOfWork.BudgetGroups.ExistAsync(
				q => q.Code == codeNorm || q.Name.ToLower() == nameNorm.ToLower()!,
				cancellationToken
			);
			if (existed) throw new ConflictException("Nhóm ngân sách đã tồn tại");

			var newGroup = new BudgetGroup(codeNorm, nameNorm);

			await _unitOfWork.BudgetGroups.AddAsync(newGroup, cancellationToken);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}

	public sealed class CreateBudgetGroupCommandValidator : AbstractValidator<CreateBudgetGroupCommand>
	{
		public CreateBudgetGroupCommandValidator()
		{
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
