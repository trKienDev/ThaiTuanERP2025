using AutoMapper;
using ThaiTuanERP2025.Application.Finance.CashoutCodes;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Application.Finance.CashoutCodes.Contracts;

namespace ThaiTuanERP2025.Infrastructure.Finance.Repositories.Read
{
	public class CashoutCodeReadRepository : BaseReadRepository<CashoutCode, CashoutCodeDto>, ICashoutCodeReadRepository
	{
		public CashoutCodeReadRepository(ThaiTuanERP2025DbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }
	}
}
