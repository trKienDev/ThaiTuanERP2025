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
			var name = command.Name.Trim();
			Guard.AgainstNullOrWhiteSpace(name, nameof(name));	

			var code = command.Code.Trim();
			Guard.AgainstNullOrWhiteSpace(code, nameof(code));

			var existed = await _unitOfWork.BudgetGroups.ExistAsync(
				q => string.Equals(q.Code, code, StringComparison.OrdinalIgnoreCase)
					|| string.Equals(q.Name, name, StringComparison.OrdinalIgnoreCase),
				cancellationToken
			);
			if (existed) throw new ConflictException("Nhóm ngân sách đã tồn tại");

			var newGroup = new BudgetGroup(code, name);

			await _unitOfWork.BudgetGroups.AddAsync(newGroup, cancellationToken);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
