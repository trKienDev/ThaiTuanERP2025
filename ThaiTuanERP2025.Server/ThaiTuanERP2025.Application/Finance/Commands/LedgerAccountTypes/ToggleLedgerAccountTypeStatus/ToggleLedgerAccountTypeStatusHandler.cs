using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Finance.Commands.LedgerAccountTypes.ToggleLedgerAccountTypeStatus
{
	public class ToggleLedgerAccountTypeStatusHandler : IRequestHandler<ToggleLedgerAccountTypeStatusCommand, bool>
	{
		private readonly IUnitOfWork _unitOfWork;
		public ToggleLedgerAccountTypeStatusHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<bool> Handle(ToggleLedgerAccountTypeStatusCommand command, CancellationToken cancellationToken)
		{
			var entity = await _unitOfWork.LedgerAccountTypes.GetByIdAsync(command.Id)
				?? throw new NotFoundException("Không tìm thấy loại tài khoản kế toán");
			
			entity.IsActive = command.IsActive;
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return true;
		}
	}
}
