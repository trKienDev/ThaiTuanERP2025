using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Finance.Commands.BudgetCodes.UpdateBudgetCodeStatus
{
	public class UpdateBudgetCodeStatusCommandHandler : IRequestHandler<UpdateBudgetCodeStatusCommand>
	{
		private readonly IUnitOfWork _unitOfWork;
		public UpdateBudgetCodeStatusCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(UpdateBudgetCodeStatusCommand request, CancellationToken cancellationToken)
		{
			var budgetCode = await _unitOfWork.BudgetCodes.GetByIdAsync(request.Id);
			if (budgetCode == null)
			{
				throw new NotFoundException("Mã ngân sách không tồn tại");
			}
			budgetCode.IsActive = request.IsActive;
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
