using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Finance.Commands.CashOutGroups.DeleteCashOutGroup
{
	public class DeleteCashOutGroupHandler : IRequestHandler<DeleteCashOutGroupCommand, bool>
	{
		private readonly IUnitOfWork _unitOfWork;
		public DeleteCashOutGroupHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<bool> Handle(DeleteCashOutGroupCommand request, CancellationToken cancellationToken) {
			var entity = await _unitOfWork.CashOutGroups.SingleOrDefaultIncludingAsync(x =>  
				x.Id == request.Id,
				asNoTracking: false,
				cancellationToken
			);

			if (entity is null) throw new NotFoundException("Không tìm thấy nhóm tài khoản đầu ra");
			_unitOfWork.CashOutGroups.Delete(entity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return true;
		}
	}
}
