using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Finance.Commands.Taxes.ToggleTaxStatus
{
	public class ToggleTaxStatusHandler : IRequestHandler<ToggleTaxStatusCommand, bool>
	{
		private readonly IUnitOfWork _unitOfWork;
		public ToggleTaxStatusHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;	
		}

		public async Task<bool> Handle(ToggleTaxStatusCommand request, CancellationToken cancellationToken) { 
			var entity = await _unitOfWork.Taxes.SingleOrDefaultIncludingAsync(x => 
				x.Id == request.Id,
				asNoTracking: false,
				cancellationToken
			);
			if (entity is null) throw new NotFoundException("Chính sách thuế không tồn tại");
			
			entity.IsActive = request.IsActive;
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return true;
		}
	}
}
