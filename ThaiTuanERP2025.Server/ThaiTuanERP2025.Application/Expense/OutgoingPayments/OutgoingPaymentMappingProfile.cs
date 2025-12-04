using AutoMapper;
using ThaiTuanERP2025.Application.Expense.OutgoingPayments.Contracts;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Shared.Utils;

namespace ThaiTuanERP2025.Application.Expense.OutgoingPayments
{
        public sealed class OutgoingPaymentMappingProfile : Profile
        {
                public OutgoingPaymentMappingProfile()
                {
                        CreateMap<OutgoingPayment, OutgoingPaymentDto>();

                        CreateMap<OutgoingPayment, OutgoingPaymentLookupDto>()
                                .ForMember(dest => dest.PostingAt, opt => opt.MapFrom(src => TimeZoneConverter.ToVietnamTime(src.PostingAt)));
		}
        }
}
