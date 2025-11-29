using FluentValidation;
using MediatR;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Application.Shared.Interfaces;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Expense.ExpenseWorkflows.Commands
{
	public sealed record ApproveExpenseStepInstanceCommand(Guid ExpenseStepInstanceId) : IRequest<Unit>;

	public sealed class ApproveExpenseStepInstanceCommandHandler : IRequestHandler<ApproveExpenseStepInstanceCommand, Unit>
	{
		private readonly IUnitOfWork _uow;
		private readonly ICurrentUserService _currentUser;
		public ApproveExpenseStepInstanceCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser)
		{
			_uow = uow;
			_currentUser = currentUser;
		}
			
		public async Task<Unit> Handle(ApproveExpenseStepInstanceCommand command, CancellationToken cancellationToken)
		{
			// === Validate ====
			var userId = _currentUser.UserId ?? throw new AppException("Tài khoản của bạn không hợp lệ");
			var userExist = await _uow.Users.ExistAsync(q => q.Id == userId && q.IsActive, cancellationToken);
			if (!userExist) throw new UnauthorizedException("Tài khoản của bạn không hợp lệ");

			var stepInstance = await _uow.ExpenseStepInstances.SingleOrDefaultAsync(
				q => q.Where(x => x.Id == command.ExpenseStepInstanceId),
				asNoTracking: false,
				cancellationToken: cancellationToken
			) ?? throw new NotFoundException("Không tìm thấy bước duyệt");

			var approverIds = stepInstance.GetResolvedApproverIds();
			if (!approverIds.Contains(userId))
				throw new BusinessRuleViolationException("Bạn không có quyền duyệt");

			stepInstance.Approve(userId);

			await _uow.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}

	public sealed class ApproveExpenseStepInstanceCommandValidator : AbstractValidator<ApproveExpenseStepInstanceCommand>
	{
		public ApproveExpenseStepInstanceCommandValidator()
		{
			RuleFor(x => x.ExpenseStepInstanceId).NotEmpty().WithMessage("Định danh bước duyệt không được để trống");
		}
	}
}
