using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Finance.Commands.Taxes.DeleteTax
{
	public class DeleteTaxHandler : IRequestHandler<DeleteTaxCommand, bool>
	{
		private readonly IUnitOfWork _unitOfWork;
		public DeleteTaxHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<bool> Handle(DeleteTaxCommand request, CancellationToken cancellationToken) {
			var entity = await _unitOfWork.Taxes.SingleOrDefaultIncludingAsync(x => 
				x.Id == request.Id, 
				asNoTracking: false, cancellationToken
			);
			if (entity is null) throw new NotFoundException("Không tìm thấy chính sách thuế");
			_unitOfWork.Taxes.Delete(entity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return true;
		}
	}
}
