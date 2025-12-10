using FluentValidation;
using MediatR;
using ThaiTuanERP2025.Application.Finance.CashoutGroups.Contracts;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Domain.Shared.Extensions;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Finance.CashoutGroups.Commands
{
	public sealed record CreateCashoutGroupCommand(CashoutGroupPayload Payload) : IRequest<Unit>;

	public sealed class CreateCashoutGroupCommandHandler : IRequestHandler<CreateCashoutGroupCommand, Unit>
	{
		private readonly IUnitOfWork _uow;
		private readonly ICashoutGroupReadRepository _cashoutGroupRepo;
		public CreateCashoutGroupCommandHandler(IUnitOfWork uow, ICashoutGroupReadRepository cashoutGroupRepo)
		{
			_uow = uow;
			_cashoutGroupRepo = cashoutGroupRepo;
		}

		public async Task<Unit> Handle(CreateCashoutGroupCommand command, CancellationToken cancellationToken) {
			var payload = command.Payload;

			var normalizedName = payload.Name.Trim();

			CashoutGroup? parent = null;
			if (payload.ParentId.HasValue)
			{
				parent = await _uow.CashoutGroups.SingleOrDefaultAsync(
					q => q.Active().Where(x => x.Id == payload.ParentId.Value),
					cancellationToken: cancellationToken
				) ?? throw new NotFoundException("Không tìm thấy nhóm cha");
			}

			var exist = await _uow.CashoutGroups.ExistAsync(
				q => q.Name == normalizedName,
				cancellationToken: cancellationToken
			);
			if ( exist ) throw new BusinessRuleViolationException("Nhóm khoản iền ra này đã tồn tại");

			var entity = new CashoutGroup(payload.Name, payload?.ParentId, payload?.Description);
			
			var maxOrderNumber = await _cashoutGroupRepo.GetMaxOrderNumberAsync(payload?.ParentId, cancellationToken);
			entity.SetParent(parent, maxOrderNumber + 1);

			await _uow.CashoutGroups.AddAsync(entity, cancellationToken);
			await _uow.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}

		public sealed class CreateCashoutGroupCommandValidator : AbstractValidator<CreateCashoutGroupCommand>
		{
			public CreateCashoutGroupCommandValidator()
			{
				RuleFor(x => x.Payload.Name)
					.NotEmpty().WithMessage("Tên nhóm khoản tiền ra không được để trống");
			}
		}
	}
}
