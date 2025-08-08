using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Finance.Commands.BudgetPlans.DeleteBudgetPlan
{
	public class DeleteBudgetPlanCommandHandler : IRequestHandler<DeleteBudgetPlanCommand, Unit>
	{
		private readonly IUnitOfWork _unitOfWork;
		public DeleteBudgetPlanCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(DeleteBudgetPlanCommand command, CancellationToken cancellationToken) {
			var plan = await _unitOfWork.BudgetPlans.GetByIdAsync(command.Id)
				?? throw new NotFoundException("Kế hoạch ngân sách không tồn tại");

			_unitOfWork.BudgetPlans.Delete(plan);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			
			return Unit.Value;
		}
	}
}
