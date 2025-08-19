using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Finance.Commands.LedgerAccountTypes.DeleteLedgerAccountType
{
	public class DeleteLedgerAccountTypeHandler : IRequestHandler<DeleteLedgerAccountTypeCommand, bool>
	{
		private readonly IUnitOfWork _unitOfWork;
		public DeleteLedgerAccountTypeHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<bool> Handle(DeleteLedgerAccountTypeCommand command, CancellationToken cancellationToken)
		{
			var entity = await _unitOfWork.LedgerAccountTypes.GetByIdAsync(command.Id)
				?? throw new NotFoundException("Không tìm thấy loại tài khoản kế toán");
			_unitOfWork.LedgerAccountTypes.Delete(entity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return true;
		}
	}
}
