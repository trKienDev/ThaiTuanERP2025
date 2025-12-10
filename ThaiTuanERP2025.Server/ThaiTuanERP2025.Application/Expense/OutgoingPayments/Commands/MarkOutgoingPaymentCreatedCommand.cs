using FluentValidation;
using MediatR;
using ThaiTuanERP2025.Application.Core.Notifications;
using ThaiTuanERP2025.Application.Expense.ExpensePayments.Repositories;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Application.Shared.Interfaces;
using ThaiTuanERP2025.Domain.Shared.Repositories;
using ValidationException = ThaiTuanERP2025.Application.Shared.Exceptions.ValidationException;

namespace ThaiTuanERP2025.Application.Expense.OutgoingPayments.Commands
{
        public sealed record MarkOutgoingPaymentCreatedCommand(Guid Id) : IRequest<Unit>;
        public sealed class MarkOutgoingPaymentCreatedCommandHandler : IRequestHandler<MarkOutgoingPaymentCreatedCommand, Unit> {
                private readonly IUnitOfWork _uow;
                private readonly INotificationService _notificationService;
                private readonly ICurrentUserService _currentUser;
                private readonly IExpensePaymentReadRepository _expensePaymentRepo;
                public MarkOutgoingPaymentCreatedCommandHandler(
                        IUnitOfWork uow, INotificationService notificationService, ICurrentUserService currentUser,
                        IExpensePaymentReadRepository expensePaymentRepo
                )
                {
                        _uow = uow;
                        _notificationService = notificationService;
                        _currentUser = currentUser;
                        _expensePaymentRepo = expensePaymentRepo;
                }

                public async Task<Unit> Handle(MarkOutgoingPaymentCreatedCommand command, CancellationToken cancellationToken)
                {
                        var userId = _currentUser.UserId ?? throw new ValidationException("User không hợp lệ");

                        var outgoingPayment = await _uow.OutgoingPayments.SingleOrDefaultAsync(
                                q => q.Where(x => x.Id == command.Id),
                                asNoTracking: false,
                                cancellationToken: cancellationToken
                        ) ?? throw new NotFoundException("Không tìm thấy tài khoản tiền ra");

                        outgoingPayment.MarkCreated(userId);
                        await _uow.SaveChangesAsync(cancellationToken);

                        var expensePaymentId = outgoingPayment.ExpensePaymentId;
                        var expensePaymentName = await _expensePaymentRepo.GetNameAsync(expensePaymentId, cancellationToken);
                        var expensePaymentCreatorId = await _expensePaymentRepo.GetCreatorIdAsync(expensePaymentId, cancellationToken);

                        if (expensePaymentCreatorId is not null)
                                await _notificationService.SendAsync(
                                        senderId: userId,
                                        receiverId: expensePaymentCreatorId.Value,
                                        title: $"{outgoingPayment.Name} đã tạo lệnh",
                                        message: $"{outgoingPayment.Name} đã tạo lệnh",
                                        linkType: Domain.Core.Enums.LinkType.OutgoingPaymentCreated,
                                        targetId: outgoingPayment.Id,
                                        type: Domain.Core.Enums.NotificationType.Info,
                                        cancellationToken: cancellationToken
                                );

                        return Unit.Value;
                }
        }

        public sealed class MarkOutgoingPaymentCreatedCommandValidator : AbstractValidator<MarkOutgoingPaymentCreatedCommand>
        {
                public MarkOutgoingPaymentCreatedCommandValidator()
                {
                        RuleFor(x => x.Id).NotEmpty().WithMessage("Định danh khoản chi không được để trống");
                }
        }
}
