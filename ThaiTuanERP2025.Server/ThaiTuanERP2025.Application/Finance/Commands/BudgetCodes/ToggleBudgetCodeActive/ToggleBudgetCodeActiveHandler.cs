using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Finance.Commands.BudgetCodes.ToggleBudgetCodeActive
{
	public class ToggleBudgetCodeActiveHandler : IRequestHandler<ToggleBudgetCodeActiveCommand>
	{
		private readonly IUnitOfWork _unitOfWork;
		public ToggleBudgetCodeActiveHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(ToggleBudgetCodeActiveCommand command, CancellationToken cancellationToken)
		{
			var budgetCode = await _unitOfWork.BudgetCodes.GetByIdAsync(command.Id) 
				?? throw new NotFoundException("Mã ngân sách không tồn tại");

			if (budgetCode.IsActive == true)
				budgetCode.Deactivate();
			else if (budgetCode.IsActive == false)
				budgetCode.Activate();
			
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
