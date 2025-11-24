using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Expense.Repositories;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Expense.Repositories.Write
{
	public class ExpenseWorkflowTemplateWriteRepository : BaseWriteRepository<ExpenseWorkflowTemplate>, IExpenseWorkflowTemplateWriteRepository
	{
		public ExpenseWorkflowTemplateWriteRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider configurationProvider) : base(dbContext, configurationProvider)
		{
		}

		public async Task<bool> ExistsActiveForScopeAsync(CancellationToken cancellationToken = default) {
			var query = _dbSet.AsNoTracking()
				.Where(x => !x.IsDeleted && x.IsActive);

			return await query.AnyAsync();
		}
		
		public async Task<List<ExpenseWorkflowTemplate>> ListByFilterAsync(bool? isActive, CancellationToken cancellationToken = default) {
			var query = _dbSet.AsNoTracking().Where(x => !x.IsDeleted);


			if (isActive.HasValue)
				query = query.Where(x => x.IsActive == isActive.Value);

			return await query.OrderByDescending(x => x.IsActive)
				.ThenBy(x => x.Name)
				.ToListAsync(cancellationToken);
		}
	}
}
