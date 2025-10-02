using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Finance.Commands.BudgetPeriods.UpdateBudgetPeriod
{
	public class UpdateBudgetPeriodCommandHandler : IRequestHandler<UpdateBudgetPeriodCommand>
	{
		private readonly IUnitOfWork _unitOfWork;
		public UpdateBudgetPeriodCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(UpdateBudgetPeriodCommand request, CancellationToken cancellationToken)
		{
			var budgetPeriod = await _unitOfWork.BudgetPeriods.GetByIdAsync(request.Id);
			if (budgetPeriod == null)
			{
				throw new NotFoundException("Kỳ ngân sách không tồn tại");
			}

			budgetPeriod.IsActive = request.IsActice;
			budgetPeriod.UpdatedDate = DateTime.UtcNow;

			_unitOfWork.BudgetPeriods.Update(budgetPeriod);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
