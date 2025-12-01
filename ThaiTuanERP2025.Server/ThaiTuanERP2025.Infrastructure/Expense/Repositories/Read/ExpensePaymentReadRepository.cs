using AutoMapper;
using AutoMapper.QueryableExtensions;
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

		public async Task<Guid?> GetCreatorIdAsync(Guid expensePaymentId, CancellationToken cancellationToken = default)
		{
			return await _dbSet.AsNoTracking()
				.Where(x => x.Id == expensePaymentId)
				.Select(x => x.CreatedByUserId)
				.SingleOrDefaultAsync(cancellationToken);
		}

		public async Task<ExpensePaymentLookupDto?> GetLookupById(Guid id, CancellationToken cancellationToken = default)
		{
			return await _dbSet.AsNoTracking()
				.Where(x => x.Id == id)
				.ProjectTo<ExpensePaymentLookupDto>(_mapperConfig)
				.SingleOrDefaultAsync(cancellationToken);
		}

		public async Task<ExpensePaymentDetailDto?> GetDetailById(Guid id, CancellationToken cancellationToken = default)
		{
			return await _dbSet.AsNoTracking()
				.Where(x => x.Id == id)
				.ProjectTo<ExpensePaymentDetailDto>(_mapperConfig)
				.SingleOrDefaultAsync(cancellationToken);
		}
	}
}
