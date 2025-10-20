using AutoMapper;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Mappings
{
	public sealed class OutgoingBankAccountMappingProfile : Profile
	{
		public OutgoingBankAccountMappingProfile() {
			CreateMap<OutgoingBankAccount, OutgoingBankAccountDto>();
		}
	}
}
