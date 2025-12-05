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

                        CreateMap<OutgoingPayment, OutgoingPaymentBriefDto>()
                                .ForMember(dest => dest.PostingAt, opt => opt.MapFrom(src => TimeZoneConverter.ToVietnamTime(src.PostingAt)));

                        CreateMap<OutgoingPayment, OutgoingPaymentLookupDto>()
                                .ForMember(dest => dest.ExpensePaymentName, opt => opt.MapFrom(src => src.ExpensePayment.Name))
                                .ForMember(dest => dest.OutgoingBankAccountName, opt => opt.MapFrom(src => src.OutgoingBankAccount.Name))
				.ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier != null ? src.Supplier.Name : string.Empty))
				.ForMember(dest => dest.DueAt, opt => opt.MapFrom(src => TimeZoneConverter.ToVietnamTime(src.DueAt)))
                                .ForMember(dest => dest.PostingAt, opt => opt.MapFrom(src => TimeZoneConverter.ToVietnamTime(src.PostingAt)));

			CreateMap<OutgoingPayment, OutgoingPaymentDetailDto>()
				.ForMember(dest => dest.ExpensePaymentName, opt => opt.MapFrom(src => src.ExpensePayment.Name))
                                .ForMember(dest => dest.ExpensePaymentAmount, opt => opt.MapFrom(src => src.ExpensePayment.TotalAmount))
                                .ForMember(dest => dest.ExpensePaymentTaxAmount, opt => opt.MapFrom(src => src.ExpensePayment.TotalTax))
                                .ForMember(dest => dest.ExpensePaymentTotalWithTax, opt => opt.MapFrom(src => src.ExpensePayment.TotalWithTax))
                                .ForMember(dest => dest.ExpensePaymentItems, opt => opt.MapFrom(src => src.ExpensePayment.Items))
                                .ForMember(dest => dest.OutgoingBankAccountName, opt => opt.MapFrom(src => src.OutgoingBankAccount.Name))
				.ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier != null ? src.Supplier.Name : string.Empty))
				.ForMember(dest => dest.DueAt, opt => opt.MapFrom(src => TimeZoneConverter.ToVietnamTime(src.DueAt)))
				.ForMember(dest => dest.PostingAt, opt => opt.MapFrom(src => TimeZoneConverter.ToVietnamTime(src.PostingAt)));
		}
        }
}
