using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Common.Services;
using ThaiTuanERP2025.Application.Followers.Services;
using ThaiTuanERP2025.Domain.Common.Enums;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Followers.Enums;

namespace ThaiTuanERP2025.Application.Expense.Commands.OutgoingPayments.CreateOutgoingPayment
{
	public class CreateOutgoingPaymentHandler : IRequestHandler<CreateOutgoingPaymentCommand, Unit>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IFollowerService _followerService;
		private readonly ICurrentUserService _currentUserService;
		private readonly IDocumentSubIdGeneratorService _documentSubIdGeneratorService;
		public CreateOutgoingPaymentHandler(
			IUnitOfWork unitOfWork, IFollowerService followerService, ICurrentUserService currentUserService,
			IDocumentSubIdGeneratorService documentSubIdGeneratorService
		)
		{
			_unitOfWork = unitOfWork;
			_followerService = followerService;
			_currentUserService = currentUserService;
			_documentSubIdGeneratorService = documentSubIdGeneratorService;
		}

		public async Task<Unit> Handle(CreateOutgoingPaymentCommand command, CancellationToken cancellationToken) {
			var currentUserId = _currentUserService.UserId ?? throw new NotFoundException("Bạn không có quyền truy cập");

			var request = command.Request;
			var existing = await _unitOfWork.OutgoingPayments.SingleOrDefaultIncludingAsync(
				x => x.Name == request.Name,
				cancellationToken: cancellationToken
			);

			var expensePayment = await _unitOfWork.ExpensePayments.SingleOrDefaultIncludingAsync(
				q => q.Id == request.ExpensePaymentId,
				cancellationToken: cancellationToken
			) ?? throw new NotFoundException("Không tìm thấy chi phí yêu cầu");

			var name = request.Name.IsNormalized() ? request.Name : request.Name.Trim();
			var bankName = request.BankName.IsNormalized() ? request.BankName : request.BankName.Trim();
			var accountNumber = request.AccountNumber.IsNormalized() ? request.AccountNumber : request.AccountNumber.Trim();
			var beneficiaryName = request.BeneficiaryName.IsNormalized() ? request.BeneficiaryName : request.BeneficiaryName.Trim();
			var subId = await _documentSubIdGeneratorService.NextSubIdAsync(DocumentType.OutgoingPayment, DateTime.UtcNow, cancellationToken);				

			if (existing is not null)
			{
				throw new InvalidOperationException("Khoản chi này đã tồn tại");
			}

			var entity = new OutgoingPayment(
				name, request.OutgoingAmount,
				bankName, accountNumber, beneficiaryName,
				request.DueDate,
				request.OutgoingBankAccountId,
				request.ExpensePaymentId,
				request.Description
			);
			entity.SetSubId(subId);
			entity.Approve(currentUserId);

			if (request.SupplierId.HasValue && !request.EmployeeId.HasValue)
				entity.SetSupplierId(request.SupplierId.Value);
			else if(request.EmployeeId.HasValue && !request.SupplierId.HasValue)
				entity.SetEmployeeId(request.EmployeeId.Value);
			else if(request.SupplierId.HasValue && request.EmployeeId.HasValue)
				throw new InvalidOperationException("Chỉ được chọn một trong Nhà cung cấp hoặc Nhân viên.");
			else if(!request.SupplierId.HasValue && !request.EmployeeId.HasValue)
				throw new InvalidOperationException("Phải chọn một trong Nhà cung cấp hoặc Nhân viên.");

			var paymentFollowerIds = await _unitOfWork.Followers.ListAsync(
				q => q.Where(f => f.SubjectType == SubjectType.ExpensePayment && f.SubjectId == expensePayment.Id).Select(f => f.UserId),
				cancellationToken: cancellationToken
			);

			var followerIds = new HashSet<Guid>() { currentUserId };
			followerIds.UnionWith(paymentFollowerIds); 
			await _followerService.FollowManyAsync(SubjectType.OutgoingPayment, entity.Id, followerIds, cancellationToken);

			await _unitOfWork.OutgoingPayments.AddAsync(entity);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}

	}
}
