using FluentValidation;
using MediatR;

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
