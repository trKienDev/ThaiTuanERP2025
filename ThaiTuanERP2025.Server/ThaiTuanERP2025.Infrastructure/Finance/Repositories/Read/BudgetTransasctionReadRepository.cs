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

                public async Task<Dictionary<Guid, decimal>> GetRemainingByDetailIdsAsync(IEnumerable<Guid> detailIds, CancellationToken cancellationToken)
                {
                        var ctx = (ThaiTuanERP2025DbContext)_dbContext;

                        // BudgetPlanDetail
                        var details = await ctx.BudgetPlanDetails
                                .Where(x => detailIds.Contains(x.Id))
                                .Select(x => new
                                {
                                        x.Id,
                                        x.Amount
                                })
                                .ToListAsync(cancellationToken);

                        // 2. Lấy giao dịch Debit - Credit
                        var spending = await _dbSet
                                .Where(x => detailIds.Contains(x.BudgetPlanDetailId))
                                .GroupBy(x => x.BudgetPlanDetailId)
                                .Select(g => new
                                {
                                        DetailId = g.Key,
                                        NetSpent = g.Sum(x => x.Type == BudgetTransactionType.Debit ? x.Amount : -x.Amount)
                                })
                                .ToListAsync(cancellationToken);

                        var spentDict = spending.ToDictionary(x => x.DetailId, x => x.NetSpent);

                        // 3. Tính Remaining = Amount - NetSpent
                        var result = new Dictionary<Guid, decimal>();
                        foreach (var d in details)
                        {
                                var used = spentDict.GetValueOrDefault(d.Id, 0m);
                                result[d.Id] = d.Amount - used;
                        }

                        return result;
                }
        }
}
