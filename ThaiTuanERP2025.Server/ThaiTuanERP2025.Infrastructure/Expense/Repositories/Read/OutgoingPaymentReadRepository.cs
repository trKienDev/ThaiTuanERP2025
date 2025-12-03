using AutoMapper;
using ThaiTuanERP2025.Application.Expense.OutgoingPayments;
using ThaiTuanERP2025.Application.Expense.OutgoingPayments.Contracts;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;

namespace ThaiTuanERP2025.Infrastructure.Expense.Repositories.Read
{
        public sealed class OutgoingPaymentReadRepository : BaseReadRepository<OutgoingPayment, OutgoingPaymentDto>, IOutgoingPaymentReadRepository
        {
                public OutgoingPaymentReadRepository(ThaiTuanERP2025DbContext dbContext, IMapper mapperConfig) : base(dbContext, mapperConfig) { }
        }
}
