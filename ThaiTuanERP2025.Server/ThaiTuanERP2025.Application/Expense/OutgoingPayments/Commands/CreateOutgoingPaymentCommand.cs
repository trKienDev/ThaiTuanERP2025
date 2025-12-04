using FluentValidation;
using MediatR;
using ThaiTuanERP2025.Application.Core.Followers;
using ThaiTuanERP2025.Application.Core.Notifications;
using ThaiTuanERP2025.Application.Expense.OutgoingPayments.Contracts;
using ThaiTuanERP2025.Application.Shared.Services;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Shared.Enums;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Expense.OutgoingPayments.Commands
{
	public sealed record CreateOutgoingPaymentCommand(OutgoingPaymentPayload Payload) : IRequest<Unit>;
	public sealed class CreateOutgoingPaymentCommandHandler : IRequestHandler<CreateOutgoingPaymentCommand, Unit>
	{
		private readonly IUnitOfWork _uow;
		private readonly IDocumentSubIdGeneratorService _documentSubIdGenerator;
		private readonly IFollowerService _followerService;
		private readonly INotificationService _notificationService;
		private readonly IFollowerReadRepository _followerRepo;
		public CreateOutgoingPaymentCommandHandler(
			IUnitOfWork uow, IDocumentSubIdGeneratorService documentSubIdGenerator, IFollowerService followerService,
			INotificationService notificationService, IFollowerReadRepository followerRepo
		) {
			_uow = uow;
			_documentSubIdGenerator = documentSubIdGenerator;
			_followerService = followerService;
			_notificationService = notificationService;
			_followerRepo = followerRepo;
		}

		public async Task<Unit> Handle(CreateOutgoingPaymentCommand command, CancellationToken cancellationToken)
		{
			var payload = command.Payload;

			#region Normalization
			var nameNorm = payload.Name.Trim();
			var bankNameNorm = payload.BankName.Trim();
			var accountNumberNorm = payload.AccountNumber.Trim();
			var beneficiaryNameNorm = payload.BeneficiaryName.Trim();
			#endregion

			#region Validation
			// Check exist OutgoingPayment by name
			var exist = await _uow.OutgoingPayments.ExistAsync(
				q => q.Name == nameNorm, cancellationToken
			);
			if (exist) throw new Shared.Exceptions.ValidationException($"Khoản chi {nameNorm} đã tồn tại");

			// Validate ExpensePayment Amount
			var expensePayment = await _uow.ExpensePayments.SingleOrDefaultAsync(
				q => q.Where(x => x.Id == payload.ExpensePaymentId), 
				asNoTracking: false,
				cancellationToken: cancellationToken
			);
			if (expensePayment == null)
				throw new Shared.Exceptions.ValidationException("Khoản thanh toán không hợp lệ");

			expensePayment.RegisterOutgoingPayment(payload.OutgoingAmount);

			#endregion

			#region Create entitty OutgoingPayment
			var newOutgoingPayment = new OutgoingPayment(
				nameNorm,
				payload.OutgoingAmount,
				bankNameNorm,
				accountNumberNorm,
				beneficiaryNameNorm,
				payload.DueAt,
				payload.OutgoingBankAccountId,
				payload.ExpensePaymentId,
				payload.Description
			);

			var subId = await _documentSubIdGenerator.NextSubIdAsync(Domain.Shared.Enums.DocumentType.OutgoingPayment, DateTime.UtcNow, cancellationToken);
			newOutgoingPayment.SetSubId(subId);

			if (expensePayment.SupplierId is not null)
				newOutgoingPayment.AssignSupplier(expensePayment.SupplierId.Value);

			await _uow.OutgoingPayments.AddAsync(newOutgoingPayment, cancellationToken);
			await _uow.SaveChangesAsync(cancellationToken);
                        #endregion


                        #region Add Followers
                        if (newOutgoingPayment.CreatedByUserId is not null)
				await _followerService.FollowAsync(DocumentType.OutgoingPayment, newOutgoingPayment.Id, newOutgoingPayment.CreatedByUserId.Value, cancellationToken);

			var expensePaymentFollowers = await _followerRepo.GetFollowerIdsByDocument(expensePayment.Id, DocumentType.ExpensePayment,cancellationToken);	
			await _followerService.FollowManyAsync(DocumentType.OutgoingPayment, newOutgoingPayment.Id, expensePaymentFollowers, cancellationToken);
			#endregion

			#region Notifiaction
			if (expensePayment.CreatedByUserId is not null && newOutgoingPayment.CreatedByUserId is not null)
			{
				await _notificationService.SendAsync(
					senderId: newOutgoingPayment.CreatedByUserId.Value,
					receiverId: expensePayment.CreatedByUserId.Value,
					title: $"Thanh toán {expensePayment.Name} đã được tạo khoản chi",
					message: $"Thanh toán {expensePayment.Name} đã được tạo khoản chi",
					linkType: Domain.Core.Enums.LinkType.OutgongPaymentPending,
					targetId: newOutgoingPayment.Id,
					type: Domain.Core.Enums.NotificationType.Info,
					cancellationToken: cancellationToken
				);
			}
                        #endregion

                        await _uow.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}

	public sealed class CreateOutgoingPaymentCommandValidator : AbstractValidator<CreateOutgoingPaymentCommand>
	{
		public CreateOutgoingPaymentCommandValidator()
		{
			RuleFor(x => x.Payload.Name)
				.NotEmpty().WithMessage("Tên khoản chi không được để trống.")
				.MaximumLength(256).WithMessage("Tên khoản chi không được vượt quá 256 ký tự");

			RuleFor(x => x.Payload.OutgoingAmount)
				.NotEmpty().WithMessage("Số tiền khoản chi không được phép để trống")
				.GreaterThan(1000).WithMessage("Số tiền khoản chi không nhỏ hơn 1.000");

			RuleFor(x => x.Payload.BankName)
				.NotEmpty().WithMessage("Tên ngân hàng thụ hưởngh không được để trống.")
				.MaximumLength(256).WithMessage("Tên ngân hàng thụ hưởng không được vượt quá 128 ký tự");

			RuleFor(x => x.Payload.AccountNumber)
				.NotEmpty().WithMessage("Số tài khoản thụ hưởngh không được để trống.")
				.MaximumLength(256).WithMessage("Số tài khoản thụ hưởng không được vượt quá 64 ký tự");

			RuleFor(x => x.Payload.BeneficiaryName)
				.NotEmpty().WithMessage("Tên người thụ hưởngh không được để trống.")
				.MaximumLength(256).WithMessage("Tên người thụ hưởng không được vượt quá 256 ký tự");

			RuleFor(x => x.Payload.DueAt)
				.NotEmpty().WithMessage("Hạn thanh toán không được để trống")
				.Must(dueAt => dueAt.Date >= DateTime.Today).WithMessage("Hạn thanh toán không được phép nhỏ hơn ngày hôm nay"); ;

			RuleFor(x => x.Payload.OutgoingBankAccountId)
				.NotEmpty().WithMessage("Định danh tài khoản tiền ra không được trống");

			RuleFor(x => x.Payload.ExpensePaymentId)
				.NotEmpty().WithMessage("Định danh thanh toán không được hợp lệ");

			RuleFor(x => x.Payload.Description)
				.MaximumLength(2048).WithMessage("Mô tả không vượt quá 2048 ký tự");
			
		}
	}

}