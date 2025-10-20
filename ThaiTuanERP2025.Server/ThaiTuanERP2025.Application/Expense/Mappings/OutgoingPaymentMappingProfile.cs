using AutoMapper;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Mappings
{
	public class OutgoingPaymentMappingProfile : Profile
	{
		public OutgoingPaymentMappingProfile() {
			CreateMap<OutgoingPayment, OutgoingPaymentDto>();

			CreateMap<OutgoingPayment, OutgoingPaymentDetailDto>()
				.ForMember(d => d.CreatedByUserId, opt => opt.MapFrom(s => s.CreatedByUserId))
				.ForMember(d => d.CreatedByUser, opt => opt.MapFrom(s => s.CreatedByUser))
				.ForMember(d => d.OutgoingBankAccount, opt => opt.MapFrom(s => s.OutgoingBankAccount))
				.ForMember(d => d.ExpensePayment, opt => opt.MapFrom(s => s.ExpensePayment))
				.ForMember(d => d.Status, opt => opt.MapFrom(s => (int)s.Status));

			CreateMap<OutgoingPayment, OutgoingPaymentSummaryDto>()
				.ForMember(d => d.OutgoingBankAccountName, opt => opt.MapFrom(s => s.OutgoingBankAccount.Name))
				.ForMember(d => d.ExpensePaymentName, opt => opt.MapFrom(s => s.ExpensePayment.Name));
		}
	}
}
