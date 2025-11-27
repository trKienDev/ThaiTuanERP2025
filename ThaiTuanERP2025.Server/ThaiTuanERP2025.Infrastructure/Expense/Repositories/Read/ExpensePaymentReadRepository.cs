using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Application.Expense.ExpensePayments.Contracts;
using ThaiTuanERP2025.Application.Expense.ExpensePayments.Repositories;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;

namespace ThaiTuanERP2025.Infrastructure.Expense.Repositories.Read
{
	public sealed class ExpensePaymentReadRepository : BaseReadRepository<ExpensePayment, ExpensePaymentDto>, IExpensePaymentReadRepository
	{
		public ExpensePaymentReadRepository(ThaiTuanERP2025DbContext dbContext, IMapper mapperConfig) : base(dbContext, mapperConfig) { }

		public async Task<string?> GetNameAsync(Guid expensePaymentId, CancellationToken cancellationToken = default)
		{
			return await _dbSet.AsNoTracking()
				.Where(x => x.Id == expensePaymentId)
				.Select(x => x.Name)
				.SingleOrDefaultAsync(cancellationToken);
		}

		public async Task<Guid> GetCreatorIdAsync(Guid expensePaymentId, CancellationToken cancellationToken = default)
		{
			return await _dbSet.AsNoTracking()
				.Where(x => x.Id == expensePaymentId)
				.Select(x => x.Id)
				.SingleOrDefaultAsync(cancellationToken);
		}
	}
}
