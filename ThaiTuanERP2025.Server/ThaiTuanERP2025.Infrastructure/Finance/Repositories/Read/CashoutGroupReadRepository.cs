using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Application.Finance.CashoutGroups;
using ThaiTuanERP2025.Application.Finance.CashoutGroups.Contracts;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;

namespace ThaiTuanERP2025.Infrastructure.Finance.Repositories.Read
{
	public sealed class CashoutGroupReadRepository : BaseReadRepository<CashoutGroup, CashoutGroupDto>, ICashoutGroupReadRepository
	{
		public CashoutGroupReadRepository(ThaiTuanERP2025DbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

		public async Task<int> GetMaxOrderNumberAsync(Guid? parentId, CancellationToken cancellationToken)
		{
			var max = await _dbSet.Where(x => x.ParentId == parentId)
				.MaxAsync(x => (int?)x.OrderNumber, cancellationToken);

			return max ?? 0;
		}
	}
}
