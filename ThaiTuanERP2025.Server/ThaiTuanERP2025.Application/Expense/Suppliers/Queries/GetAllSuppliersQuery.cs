using MediatR;
using System.Reflection.Metadata.Ecma335;

namespace ThaiTuanERP2025.Application.Expense.Suppliers.Queries
{
	public sealed record GetAllSuppliersQuery() : IRequest<IReadOnlyList<SupplierDto>>;

	public sealed class GetAllSuppliersQueryHandler : IRequestHandler<GetAllSuppliersQuery, IReadOnlyList<SupplierDto>>
	{
		private readonly ISupplierReadRepository _supplierRepo;
		public GetAllSuppliersQueryHandler(ISupplierReadRepository supplierRepo)
		{
			_supplierRepo = supplierRepo;
		}
		
		public async Task<IReadOnlyList<SupplierDto>> Handle(GetAllSuppliersQuery query, CancellationToken cancellationToken)
		{
			return await _supplierRepo.GetAllAsync(
				q => q.IsActive,
				cancellationToken: cancellationToken
			);
		}
	}
}
