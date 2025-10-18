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
				.ForMember(d => d.Status, opt => opt.MapFrom(s => (int)s.Status));
		}
	}
}
