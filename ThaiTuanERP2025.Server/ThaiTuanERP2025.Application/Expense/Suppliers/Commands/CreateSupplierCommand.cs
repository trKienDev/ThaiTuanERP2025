using FluentValidation;
using MediatR;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Expense.Suppliers.Commands
{
	public sealed record CreateSupplierCommand(string Name, string? TaxCode) : IRequest<Guid>;

	public sealed class CreateSupplierCommandHandler : IRequestHandler<CreateSupplierCommand, Guid>
	{
		private readonly IUnitOfWork _uow;
		public CreateSupplierCommandHandler(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public async Task<Guid> Handle(CreateSupplierCommand command, CancellationToken cancellationToken)
		{
			var nameNorm = command.Name.Trim();
			string? taxCodeNorm = command.TaxCode?.Trim().ToUpperInvariant();

			if (string.IsNullOrWhiteSpace(taxCodeNorm)) taxCodeNorm = null;

			var existed = await _uow.Suppliers.ExistAsync(
				q => q.Name == nameNorm || (taxCodeNorm != null && q.TaxCode == taxCodeNorm),
				cancellationToken: cancellationToken
			);
			if (existed) throw new BusinessRuleViolationException("Nhà cung cấp này dã tồn tại");

			var newSupplier = new Supplier(nameNorm, taxCodeNorm);
			await _uow.Suppliers.AddAsync(newSupplier, cancellationToken);
			await _uow.SaveChangesAsync(cancellationToken);

			return newSupplier.Id;
		}
	}

	public sealed class CreateSupplierCommandValidator : AbstractValidator<CreateSupplierCommand>
	{
		public CreateSupplierCommandValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Tên nhà cung cấp là bắt buộc")
				.Must(v => v!.Trim().Length <= 200).WithMessage("Tên nhà cung cáp không được vượt quá 200 ký tự");
		}
	}
}
