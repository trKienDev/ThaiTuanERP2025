using MediatR;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Finance.Commands.CashOutGroups.ToggleCashOutGroupActivate;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Finance.Commands.CashoutGroups.ToggleCashoutGroupActivate
{
	public class ToggleCashoutGroupActivateHandler : IRequestHandler<ToggleCashoutGroupActivateCommand, bool>
	{
		private readonly IUnitOfWork _unitOfWork;
		public ToggleCashoutGroupActivateHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<bool> Handle(ToggleCashoutGroupActivateCommand request, CancellationToken cancellationToken) {
			var entity = await _unitOfWork.CashoutGroups.SingleOrDefaultIncludingAsync(x =>
				x.Id == request.Id,
				asNoTracking: false, 
				cancellationToken
			);
			if (entity is null) throw new NotFoundException("Không tìm thấy nhóm tài khoản đầu ra");
			entity.IsActive = request.IsActive;
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return true;
		}
	}
}
