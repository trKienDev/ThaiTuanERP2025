using AutoMapper;
using ThaiTuanERP2025.Application.Expense.Suppliers;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;

namespace ThaiTuanERP2025.Infrastructure.Expense.Repositories.Read
{
	public sealed class SupplierReadRepository : BaseReadRepository<Supplier, SupplierDto>, ISupplierReadRepository
	{	
		public SupplierReadRepository(ThaiTuanERP2025DbContext dbContext, IMapper mapperConfig) : base(dbContext, mapperConfig) { }

		public async Task<SupplierBeneficiaryInforDto> GetSupplierBeneficiaryInfor(Guid supplierId, CancellationToken cancellationToken)
		{

		}
	}
}
