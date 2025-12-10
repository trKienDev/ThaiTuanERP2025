using AutoMapper;
using ThaiTuanERP2025.Application.Finance.LedgerAccountTypes;
using ThaiTuanERP2025.Application.Finance.LedgerAccountTypes.Contracts;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;

namespace ThaiTuanERP2025.Infrastructure.Finance.Repositories.Read
{
	public sealed class LedgerAccountTypeReadRepository : BaseReadRepository<LedgerAccountType, LedgerAccountTypeDto>, ILedgerAccountTypeReadRepository
	{
		public LedgerAccountTypeReadRepository(ThaiTuanERP2025DbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }
	}
}
