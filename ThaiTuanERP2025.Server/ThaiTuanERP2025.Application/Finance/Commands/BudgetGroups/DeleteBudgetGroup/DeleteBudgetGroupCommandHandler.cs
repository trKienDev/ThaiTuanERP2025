using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Finance.Commands.BudgetGroup.DeleteBudgetGroup
{
	public class DeleteBudgetGroupCommandHandler : IRequestHandler<DeleteBudgetGroupCommand>
	{
		private readonly IUnitOfWork _unitOfWork;
		public DeleteBudgetGroupCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(DeleteBudgetGroupCommand request, CancellationToken cancellationToken)
		{
			var entity = await _unitOfWork.BudgetGroups.GetByIdAsync(request.Id)
				?? throw new NotFoundException($"Không tìm thấy nhóm ngân sách");

			_unitOfWork.BudgetGroups.Delete(entity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
