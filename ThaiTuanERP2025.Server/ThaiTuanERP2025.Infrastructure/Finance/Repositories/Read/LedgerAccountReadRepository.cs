using AutoMapper;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Application.Finance.LedgerAccounts;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;

namespace ThaiTuanERP2025.Infrastructure.Finance.Repositories.Read
{
	public sealed class LedgerAccountReadRepository : BaseReadRepository<LedgerAccount, LedgerAccountDto>, ILedgerAccountReadRepository
	{
		public LedgerAccountReadRepository(ThaiTuanERP2025DbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }
	}
}
