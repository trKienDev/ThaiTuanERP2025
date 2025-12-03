using AutoMapper;
using ThaiTuanERP2025.Application.Expense.OutgoingBankAccounts.Contracts;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.OutgoingBankAccounts
{
	public sealed class OutgoingBankAccountMappingProfile : Profile
	{
		public OutgoingBankAccountMappingProfile()
		{
			CreateMap<OutgoingBankAccount, OutgoingBankAccountDto>();
		}
	}
}
