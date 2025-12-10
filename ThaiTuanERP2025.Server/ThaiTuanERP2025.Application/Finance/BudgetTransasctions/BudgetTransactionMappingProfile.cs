using AutoMapper;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.BudgetTransasctions
{
	public sealed class BudgetTransactionMappingProfile : Profile
	{
		public BudgetTransactionMappingProfile()
		{
			CreateMap<BudgetTransaction, BudgetTransactionDto>();
		}
	}
}
