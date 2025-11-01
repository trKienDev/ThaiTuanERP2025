using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Expense.Enums;
using ThaiTuanERP2025.Domain.Expense.Repositories;
using ThaiTuanERP2025.Infrastructure.Common.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Expense.Repositories
{
	public sealed  class ExpenseWorkflowInstanceRepository : BaseRepository<ExpenseWorkflowInstance>, IExpenseWorkflowInstanceRepository
	{
		public ExpenseWorkflowInstanceRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider configurationProvider) : base(dbContext, configurationProvider) { }

		public async Task<List<ExpenseWorkflowInstance>> ListByFilterAsync(string? documentType, Guid? documentId, WorkflowStatus? status, string? budgetCode, decimal? minAmount, decimal? maxAmount, CancellationToken cancellationToken)
		{
			var query = _dbSet.AsNoTracking()
				   .Include(x => x.Steps)
				   .Where(_ => true);

			if (!string.IsNullOrWhiteSpace(documentType)) query = query.Where(x => x.DocumentType == documentType);
			if (documentId.HasValue) query = query.Where(x => x.DocumentId == documentId);
			if (status.HasValue) query = query.Where(x => x.Status == status);
			if (!string.IsNullOrWhiteSpace(budgetCode)) query = query.Where(x => x.BudgetCode == budgetCode);
			if (minAmount.HasValue) query = query.Where(x => x.Amount >= minAmount);
			if (maxAmount.HasValue) query = query.Where(x => x.Amount <= maxAmount);

			return await query.ToListAsync(cancellationToken);
		}
	}
}
