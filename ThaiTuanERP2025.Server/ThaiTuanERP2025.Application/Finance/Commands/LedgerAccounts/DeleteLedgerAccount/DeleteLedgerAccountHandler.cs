using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Finance.Commands.LedgerAccounts.DeleteLedgerAccount
{
	public class DeleteLedgerAccountHandler : IRequestHandler<DeleteLedgerAccountCommand, bool> {
		private readonly IUnitOfWork _unitOfWork;
		public DeleteLedgerAccountHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork;
		}

		public async Task<bool> Handle(DeleteLedgerAccountCommand command, CancellationToken cancellationToken) {
			var entity = await _unitOfWork.LedgerAccounts.SingleOrDefaultIncludingAsync(
				x => x.Id == command.Id,
				asNoTracking: false,
				cancellationToken
			);
			if (entity is null) throw new NotFoundException("Không tìm thấy tài khoản kế toán");

			_unitOfWork.LedgerAccounts.Delete(entity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return true;
		}
	}
}
