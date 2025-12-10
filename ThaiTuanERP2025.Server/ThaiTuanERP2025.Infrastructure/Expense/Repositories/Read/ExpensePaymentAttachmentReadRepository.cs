using ThaiTuanERP2025.Application.Expense.ExpensePayments.Contracts;
using ThaiTuanERP2025.Application.Expense.ExpensePayments.Repositories;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;

namespace ThaiTuanERP2025.Infrastructure.Expense.Repositories.Read
{
        public sealed class ExpensePaymentAttachmentReadRepository : BaseReadRepository<ExpensePaymentAttachment, ExpensePaymentAttachmentDto>, IExpensePaymentAttachmentReadRepository
        {
                public ExpensePaymentAttachmentReadRepository(ThaiTuanERP2025DbContext dbContext, AutoMapper.IMapper mapperConfig) : base(dbContext, mapperConfig) { }
        }
}
