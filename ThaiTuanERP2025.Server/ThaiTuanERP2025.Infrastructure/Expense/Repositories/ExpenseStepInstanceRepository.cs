using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Expense.Enums;
using ThaiTuanERP2025.Domain.Expense.Repositories;
using ThaiTuanERP2025.Infrastructure.Common.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Expense.Repositories
{
	public sealed class ExpenseStepInstanceRepository : BaseRepository<ExpenseStepInstance>, IExpenseStepInstanceRepository
	{
		public ExpenseStepInstanceRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider configurationProvider) : base(dbContext, configurationProvider) { }

		public Task<ExpenseStepInstance?> GetByIdWithWorkflowAsync(Guid stepId, CancellationToken cancellationToken = default) {
			return _dbSet.Include(s => s.WorkflowInstance)
				.FirstOrDefaultAsync(s => s.Id == stepId, cancellationToken);
		}

		public Task<ExpenseStepInstance?> GetCurrentStepAsync(Guid workflowId, CancellationToken cancellationToken = default)
		{
			return _dbSet.Where(s => s.WorkflowInstanceId == workflowId && s.Status == StepStatus.Waiting)
				.OrderBy(s => s.Order)
				.FirstOrDefaultAsync(cancellationToken);
		}
	}
}
