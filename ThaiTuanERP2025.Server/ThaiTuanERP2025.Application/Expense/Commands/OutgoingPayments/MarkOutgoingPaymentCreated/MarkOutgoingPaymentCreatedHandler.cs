using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;

namespace ThaiTuanERP2025.Application.Expense.Commands.OutgoingPayments.MarkOutgoingPaymentCreated
{
	public sealed class MarkOutgoingPaymentCreatedHandler : IRequestHandler<MarkOutgoingPaymentCreatedCommand, Unit>
	{
		private readonly ICurrentUserService currentUserService;
		private readonly IUnitOfWork _unitOfWork;
		public MarkOutgoingPaymentCreatedHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
		{
			this.currentUserService = currentUserService;
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(MarkOutgoingPaymentCreatedCommand command, CancellationToken cancellationToken)
		{
			var currentUserId = currentUserService.UserId ?? throw new Exception("Bạn không có quyền truy cập");
			
			var outgoingPayment = await _unitOfWork.OutgoingPayments.GetByIdAsync(command.Id)
				?? throw new Exception("Không tìm thấy khoản chi yêu cầu");

			if (outgoingPayment.Status != Domain.Expense.Enums.OutgoingPaymentStatus.Approved)
				throw new InvalidOperationException("Khoản chi không ở trạng thái chờ tạo lệnh.");

			outgoingPayment.MarkCreated(currentUserId);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
