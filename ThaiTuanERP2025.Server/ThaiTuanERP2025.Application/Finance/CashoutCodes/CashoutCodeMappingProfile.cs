using AutoMapper;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.CashoutCodes
{
	public class CashoutCodeMappingProfile : Profile
	{
		public CashoutCodeMappingProfile()  {
			CreateMap<CashoutCode, CashoutCodeDto>();
		}
	}
}
