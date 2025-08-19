using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Finance.Commands.CashOutCodes.ToggleCashOutCodeStatus
{
	public class ToggleCashOutCodeStatusHandler : IRequestHandler<ToggleCashOutCodeStatusCommand, bool>
	{
		private readonly IUnitOfWork _unitOfWork;
		public ToggleCashOutCodeStatusHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;	
		}

		public async Task<bool> Handle(ToggleCashOutCodeStatusCommand request, CancellationToken cancellationToken) {
			var entity = await _unitOfWork.CashOutCodes.SingleOrDefaultIncludingAsync(x =>
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
