using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Finance.Commands.CashoutGroups.DeleteCashoutGroup
{
	public class DeleteCashoutGroupHandler : IRequestHandler<DeleteCashoutGroupCommand, bool>
	{
		private readonly IUnitOfWork _unitOfWork;
		public DeleteCashoutGroupHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<bool> Handle(DeleteCashoutGroupCommand request, CancellationToken cancellationToken) {
			var entity = await _unitOfWork.CashoutGroups.SingleOrDefaultIncludingAsync(x =>  
				x.Id == request.Id,
				asNoTracking: false,
				cancellationToken
			);

			if (entity is null) throw new NotFoundException("Không tìm thấy nhóm tài khoản đầu ra");
			_unitOfWork.CashoutGroups.Delete(entity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return true;
		}
	}
}
