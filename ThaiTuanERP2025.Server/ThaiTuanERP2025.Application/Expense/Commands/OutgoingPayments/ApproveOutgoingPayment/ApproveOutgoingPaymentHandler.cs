using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Application.Expense.Commands.OutgoingPayments.ApproveOutgoingPayment
{
	public sealed class ApproveOutgoingPaymentHandler : IRequestHandler<ApproveOutgoingPaymentCommand, Unit>
	{
		private readonly ICurrentUserService _currentUserService;
		private readonly IUnitOfWork _unitOfWork;
		public ApproveOutgoingPaymentHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
		{
			_currentUserService = currentUserService;
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(ApproveOutgoingPaymentCommand command, CancellationToken cancellationToken) {
			var currentUserId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("Bạn không có quyền truy cập");

			var outgoingPayment = await _unitOfWork.OutgoingPayments.GetByIdAsync(command.Id, cancellationToken) 
				?? throw new NotFoundException("Không tìm thấy khoản chi yêu cầu");
				
			if(outgoingPayment.Status != Domain.Expense.Enums.OutgoingPaymentStatus.Pending) 
				throw new InvalidOperationException("Khoản chi không ở trạng thái chờ duyệt.");

			if(outgoingPayment.CreatedByUserId != currentUserId)
				throw new InvalidOperationException("Chỉ người tạo khoản chi mới có quyền duyệt.");

			outgoingPayment.Approve(currentUserId);

			var expensePayment = await _unitOfWork.ExpensePayments.GetByIdAsync(outgoingPayment.ExpensePaymentId, cancellationToken)
				?? throw new NotFoundException("Không tìm thấy khoản thanh toán tương ứng");
			var allOutgoingPayments = await _unitOfWork.OutgoingPayments.ListAsync(
				q => q.Where(o => o.ExpensePaymentId ==expensePayment.Id),
				asNoTracking: false,
				cancellationToken: cancellationToken
			 );

			expensePayment.UpdateOutgoingAmountPaid(allOutgoingPayments);
			expensePayment.RecalculateOutgoingRemaining();
			expensePayment.EvaluatePaymentStatus();

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
