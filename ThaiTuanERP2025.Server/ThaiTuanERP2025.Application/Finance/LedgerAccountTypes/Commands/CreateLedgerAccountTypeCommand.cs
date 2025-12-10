using FluentValidation;
using MediatR;
using ThaiTuanERP2025.Application.Finance.LedgerAccountTypes.Contracts;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Finance.LedgerAccountTypes.Commands
{
	public sealed record CreateLedgerAccountTypeCommand(LedgerAccountTypePayload Payload) : IRequest<Unit>;
	public sealed class CreateLedgerAccountTypeCommandHandler : IRequestHandler<CreateLedgerAccountTypeCommand, Unit>
	{
		private readonly IUnitOfWork _uow;
		public CreateLedgerAccountTypeCommandHandler(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public async Task<Unit> Handle(CreateLedgerAccountTypeCommand command, CancellationToken cancellationToken)
		{
			var payload = command.Payload;

			var normalizedCode = payload.Code.Trim().ToLowerInvariant();
			var normalizedName = payload.Name.Trim();

			var exist = await _uow.LedgerAccountTypes.ExistAsync(
				q => q.Name == normalizedName || q.Code == normalizedCode,
				cancellationToken: cancellationToken
			);
			if (exist) throw new BusinessRuleViolationException("Loại tài khoản này đã tồn tại");

			var entity = new LedgerAccountType(payload.Code, payload.Name, payload.Kind, payload.Description);

			await _uow.LedgerAccountTypes.AddAsync(entity, cancellationToken);
			await _uow.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}

	public sealed class CreateLedgerAccountTypeCommandValidator : AbstractValidator<CreateLedgerAccountTypeCommand>
	{
		public CreateLedgerAccountTypeCommandValidator()
		{
			RuleFor(x => x.Payload.Name)
				.NotEmpty().WithMessage("Tên loại tài khoản không được để trống");

			RuleFor(x => x.Payload.Code)
				.NotEmpty().WithMessage("Mã loại tài khoản không được để trống");
		}
	}
}
