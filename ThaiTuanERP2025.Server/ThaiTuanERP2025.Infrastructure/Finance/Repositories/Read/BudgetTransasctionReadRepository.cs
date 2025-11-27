using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Application.Finance.BudgetTransasctions;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Domain.Finance.Enums;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;

namespace ThaiTuanERP2025.Infrastructure.Finance.Repositories.Read
{
	public sealed class BudgetTransasctionReadRepository : BaseReadRepository<BudgetTransaction, BudgetTransactionDto>, IBudgetTransactionReadRepository
	{
		public BudgetTransasctionReadRepository(ThaiTuanERP2025DbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

		public async Task<decimal> GetRemainingAsync(Guid budgetPlanDetailId, CancellationToken cancellationToken)
		{
			var detail = await _dbContext.Set<BudgetPlanDetail>().FirstOrDefaultAsync(x => x.Id == budgetPlanDetailId, cancellationToken);

			if (detail == null) throw new NotFoundException("Không tìm thấy kế hoạch ngân sách");

			var spent = await _dbSet.Where(x => x.BudgetPlanDetailId == budgetPlanDetailId)
				.SumAsync(x => x.Type == BudgetTransactionType.Debit ? x.Amount : -x.Amount, cancellationToken);

			return detail.Amount - spent;
		}
	}
}
