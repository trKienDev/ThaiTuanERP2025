using FluentValidation;
using MediatR;
using ThaiTuanERP2025.Application.Shared.Exceptions;

namespace ThaiTuanERP2025.Application.Expense.Suppliers.Queries
{
        public sealed record GetSupplierBeneficiaryInforQuery(Guid SupplierId) : IRequest<SupplierBeneficiaryInforDto>;

        public sealed class GetSupplierBeneficiaryInforQueryHandler : IRequestHandler<GetSupplierBeneficiaryInforQuery, SupplierBeneficiaryInforDto>
        {
                private readonly ISupplierReadRepository _supplierRepo;
                public GetSupplierBeneficiaryInforQueryHandler(ISupplierReadRepository supplierRepo)
                {
                        _supplierRepo = supplierRepo;
                }

                public async Task<SupplierBeneficiaryInforDto> Handle(GetSupplierBeneficiaryInforQuery query, CancellationToken cancellationToken)
                {
			var supplier = await _supplierRepo.GetByIdProjectedAsync(query.SupplierId, cancellationToken);

			if (supplier is null) throw new NotFoundException("Không tìm thấy nhà cung cấp");

			return new SupplierBeneficiaryInforDto
			{
				BeneficiaryAccountNumber = supplier.BeneficiaryAccountNumber,
				BeneficiaryName = supplier.BeneficiaryName,
				BeneficiaryBankName = supplier.BeneficiaryBankName
			};
		}
        }

        public sealed class GetSupplierBeneficiaryInforQueryValidattor : AbstractValidator<GetSupplierBeneficiaryInforQuery>
        {
                public GetSupplierBeneficiaryInforQueryValidattor()
                {
                        RuleFor(x => x.SupplierId).NotEmpty().WithMessage("Định danh nhà cung cấp không được để trống");
                }
        }
}
