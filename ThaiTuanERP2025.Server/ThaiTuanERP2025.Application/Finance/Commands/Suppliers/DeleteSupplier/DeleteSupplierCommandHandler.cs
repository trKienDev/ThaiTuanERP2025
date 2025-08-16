using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Finance.Commands.Suppliers.DeleteSupplier
{
	public class DeleteSupplierCommandHandler : IRequestHandler<DeleteSupplierCommand, bool>
	{
		private readonly IUnitOfWork _unitOfWork;
		public DeleteSupplierCommandHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<bool> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
		{
			var entity = await _unitOfWork.Suppliers.GetByIdAsync(request.Id, cancellationToken);
			if (entity == null)
				throw new NotFoundException($"Nhà cung cấp với ID {request.Id} không tồn tại");
			_unitOfWork.Suppliers.Delete(entity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return true;
		}
	}
}
