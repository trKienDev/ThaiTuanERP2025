using ThaiTuanERP2025.Application.Expense.OutgoingPayments.Contracts;
using ThaiTuanERP2025.Application.Shared.Repositories;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.OutgoingPayments
{
        public interface IOutgoingPaymentReadRepository : IBaseReadRepository<OutgoingPayment, OutgoingPaymentDto>
        {
                Task<OutgoingPaymentDetailDto?> GetDetailById(Guid id, CancellationToken cancellationToken = default);
        }
}
