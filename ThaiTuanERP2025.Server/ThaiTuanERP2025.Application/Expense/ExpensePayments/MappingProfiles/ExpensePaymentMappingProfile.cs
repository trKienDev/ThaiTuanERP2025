using AutoMapper;
using ThaiTuanERP2025.Application.Expense.ExpensePayments.Contracts;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.ExpensePayments.MappingProfiles
{
	public sealed class ExpensePaymentMappingProfile : Profile { 
		public ExpensePaymentMappingProfile() {
			CreateMap<ExpensePayment, ExpensePaymentDto>();
		}
	}
}
