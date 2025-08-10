using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Finance.Commands.BankAccounts.ToggleBankAccountStatus
{
	public class ToggleBankAccountStatusCommandHandler : IRequestHandler<ToggleBankAccountStatusCommand, Unit>
	{
		private readonly IUnitOfWork _unitOfWork;
		public ToggleBankAccountStatusCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(ToggleBankAccountStatusCommand command, CancellationToken cancellationToken) {
			var entity = await _unitOfWork.BankAccounts.GetByIdAsync(command.Id)
				?? throw new NotFoundException("Tài khoản ngân hàng không tồn tại");

			entity.IsActive = command.IsActive;
			_unitOfWork.BankAccounts.Update(entity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
