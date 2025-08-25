using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Finance.Commands.CashoutCodes.DeleteCashoutCode
{
	public class DeleteCashoutCodeHandler : IRequestHandler<DeleteCashoutCodeCommand, bool>
	{
		private readonly IUnitOfWork _unitOfWork;
		public DeleteCashoutCodeHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork;
		}

		public async Task<bool> Handle(DeleteCashoutCodeCommand request, CancellationToken cancellationToken) {
			var entity = await _unitOfWork.CashoutCodes.SingleOrDefaultIncludingAsync(x => 
				x.Id == request.Id, asNoTracking: false, cancellationToken
			);
			if (entity is null) throw new NotFoundException("Không tìm thấy mã dòng tiền ra");
			_unitOfWork.CashoutCodes.Delete(entity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return true;
		}
	}
}
