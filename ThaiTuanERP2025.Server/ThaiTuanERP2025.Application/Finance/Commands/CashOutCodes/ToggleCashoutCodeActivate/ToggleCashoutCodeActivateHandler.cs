using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Finance.Commands.CashoutCodes.ToggleCashoutCodeActivate
{
	public class ToggleCashoutCodeActivateHandler : IRequestHandler<ToggleCashoutCodeActivateCommand, bool>
	{
		private readonly IUnitOfWork _unitOfWork;
		public ToggleCashoutCodeActivateHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;	
		}

		public async Task<bool> Handle(ToggleCashoutCodeActivateCommand request, CancellationToken cancellationToken) {
			var entity = await _unitOfWork.CashoutCodes.SingleOrDefaultIncludingAsync(x =>
				x.Id == request.Id, 
				asNoTracking: false
			);
			if (entity is null) throw new NotFoundException("Không tìm thấy mã dòng tiền ra");
			entity.IsActive = request.IsActive;
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return true;
		}
	}
}
