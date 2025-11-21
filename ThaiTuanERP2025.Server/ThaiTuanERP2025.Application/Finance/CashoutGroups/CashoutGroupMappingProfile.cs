using AutoMapper;
using ThaiTuanERP2025.Application.Finance.CashoutGroups.Contracts;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.CashoutGroups
{
	public sealed class CashoutGroupMappingProfile : Profile
	{
		public CashoutGroupMappingProfile()
		{
			CreateMap<CashoutGroup, CashoutGroupDto>();
			CreateMap<CashoutGroup, CashoutGroupTreeDto>();
		}
	}
}
