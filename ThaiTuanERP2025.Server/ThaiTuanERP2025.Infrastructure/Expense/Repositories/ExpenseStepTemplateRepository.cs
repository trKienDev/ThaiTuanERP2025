using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Expense.Repositories;
using ThaiTuanERP2025.Infrastructure.Common.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Expense.Repositories
{
	public sealed class ExpenseStepTemplateRepository : BaseRepository<ExpenseStepTemplate>, IExpenseStepTemplateRepository 
	{
		public ExpenseStepTemplateRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider configurationProvider) : base(dbContext, configurationProvider)
		{
		}

		public Task<bool> ExistOrderAsync(Guid workflowTemplateId, int order, Guid? excludeId = null, CancellationToken cancellationToken = default) {
			var query = _dbSet.AsNoTracking()
				.Where(x => !x.IsDeleted && x.WorkflowTemplateId == workflowTemplateId && x.Order == order);

			if(excludeId is Guid ex) query = query.Where(x => x.Id != ex);
			return query.AnyAsync(cancellationToken);
		}
	}
}
