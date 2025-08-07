using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Finance.Commands.BudgetPeriods.DeleteBudgetPeriod
{
	public class DeleteBudgetPeriodCommandHandler : IRequestHandler<DeleteBudgetPeriodCommand>
	{
		private readonly IUnitOfWork _unitOfWork;	
		public DeleteBudgetPeriodCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(DeleteBudgetPeriodCommand request, CancellationToken cancellationToken)
		{
			var budgetPeriod = await _unitOfWork.BudgetPeriods.GetByIdAsync(request.Id);
			if (budgetPeriod == null)
			{
				throw new NotFoundException($"Budget period with ID {request.Id} not found.");
			}
			 _unitOfWork.BudgetPeriods.Delete(budgetPeriod);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
