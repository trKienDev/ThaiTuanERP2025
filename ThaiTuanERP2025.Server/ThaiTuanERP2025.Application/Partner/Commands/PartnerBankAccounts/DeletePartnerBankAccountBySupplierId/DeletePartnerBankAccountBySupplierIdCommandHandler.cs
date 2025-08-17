using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Partner.Commands.PartnerBankAccounts.DeletePartnerBankAccountBySupplierId
{
	public class DeletePartnerBankAccountBySupplierIdCommandHandler : IRequestHandler<DeletePartnerBankAccountBySupplierIdCommand, bool>
	{
		private readonly IUnitOfWork _unitOfWork;
		public DeletePartnerBankAccountBySupplierIdCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<bool> Handle(DeletePartnerBankAccountBySupplierIdCommand command, CancellationToken cancellationToken) {
			var entity = await _unitOfWork.PartnerBankAccounts.GetBySupplierIdAsync(command.supplierId, cancellationToken);
			if (entity is null) throw new NotFoundException($"Nhà cung cấp '{command.supplierId}' chưa có tài khoản ngân hàng");

			_unitOfWork.PartnerBankAccounts.Delete(entity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return true;
		}
	}
}
