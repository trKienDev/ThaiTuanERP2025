using AutoMapper;
using ThaiTuanERP2025.Application.Finance.LedgerAccountTypes.Contracts;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.LedgerAccountTypes
{
	public sealed class LedgerAccountTypeMappingProfile : Profile
	{
		public LedgerAccountTypeMappingProfile() {
			CreateMap<LedgerAccountType, LedgerAccountTypeDto>()
				.ForMember(d => d.LedgerAccountTypeKind, opt => opt.MapFrom(s => (int)s.LedgerAccountTypeKind));
		}
	}
}
