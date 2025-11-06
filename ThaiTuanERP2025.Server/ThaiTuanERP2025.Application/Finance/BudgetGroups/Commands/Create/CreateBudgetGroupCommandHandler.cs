using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Exceptions;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.BudgetGroups.Commands.Create
{
	public sealed class CreateBudgetGroupCommandHandler : IRequestHandler<CreateBudgetGroupCommand, Unit>
	{
		private readonly IUnitOfWork _unitOfWork;
		public CreateBudgetGroupCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(CreateBudgetGroupCommand command, CancellationToken cancellationToken) {
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
}
