using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Finance.Commands.BankAccounts.DeleteBankAccount
{
	public class DeleteBankAccountCommandHandler : IRequestHandler<DeleteBankAccountCommand, Unit>
	{
		private readonly IUnitOfWork _unitOfWork;
		public DeleteBankAccountCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(DeleteBankAccountCommand command, CancellationToken cancellationToken)  {
			var entity = await _unitOfWork.BankAccounts.GetByIdAsync(command.Id, cancellationToken)
				?? throw new NotFoundException("Tài khoản ngân hàng không tồn tại");

			_unitOfWork.BankAccounts.Delete(entity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
