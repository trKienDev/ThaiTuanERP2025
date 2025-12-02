using ThaiTuanERP2025.Application.Expense.ExpensePayments.Contracts;
using ThaiTuanERP2025.Application.Shared.Repositories;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.ExpensePayments.Repositories
{
        public interface IExpensePaymentAttachmentReadRepository : IBaseReadRepository<ExpensePaymentAttachment, ExpensePaymentAttachmentDto>
        {
        }
}
