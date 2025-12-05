using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

                public async Task<OutgoingPaymentDetailDto?> GetDetailById(Guid id, CancellationToken cancellationToken = default)
                {
                        var outgoingDetail = await _dbSet.Include(x => x.CreatedByUser)
                                .Include(x => x.ExpensePayment).ThenInclude(e => e.Items)
                                .Include(x => x.ExpensePayment).ThenInclude(e => e.Items).ThenInclude(i => i.BudgetPlanDetail).ThenInclude(b => b.BudgetCode)
                                .Include(x => x.ExpensePayment).ThenInclude(e => e.Items).ThenInclude(i => i.InvoiceFile)
                                .Include(x => x.ExpensePayment).ThenInclude(e => e.OutgoingPayments)
                                .Include(x => x.OutgoingBankAccount)
                                .Include(x => x.Supplier)
                                .FirstOrDefaultAsync(x => x.Id == id);

                        return _mapper.Map<OutgoingPaymentDetailDto?>(outgoingDetail);
                }
	}
}
