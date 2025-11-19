using AutoMapper;
using ThaiTuanERP2025.Application.Finance.LedgerAccounts.Contracts;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.LedgerAccounts
{
	public sealed class LedgerAccountMappingProfile : Profile
	{
		public LedgerAccountMappingProfile() {
			CreateMap<LedgerAccount, LedgerAccountDto>();
		}
	}
}
