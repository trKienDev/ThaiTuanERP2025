using AutoMapper;
using ThaiTuanERP2025.Application.Expense.OutgoingPayments.Contracts;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.OutgoingPayments
{
        public sealed class OutgoingPaymentMappingProfile : Profile
        {
                public OutgoingPaymentMappingProfile()
                {
                        CreateMap<OutgoingPayment, OutgoingPaymentDto>();
                }
        }
}
