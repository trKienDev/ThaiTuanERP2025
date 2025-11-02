using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Exceptions;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.Budgets.Commands.BudgetGroups.CreateBudgetGroup
{
	public sealed class CreateBudgetGroupHandler : IRequestHandler<CreateBudgetGroupCommand, Unit>
	{
		private readonly IUnitOfWork _unitOfWork;
		public CreateBudgetGroupHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(CreateBudgetGroupCommand command, CancellationToken cancellationToken) {
			var request = command.Request;

			var exist = await _unitOfWork.BudgetGroups.AnyAsync(
				x => x.Name == request.Name && x.Code == request.Code,
				cancellationToken
			);
			if (exist) throw new ConflictException("Nhóm ngân sách đã tồn tại");

			var entity = new BudgetGroup(request.Name, request.Code);

			await _unitOfWork.BudgetGroups.AddAsync(entity, cancellationToken);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
