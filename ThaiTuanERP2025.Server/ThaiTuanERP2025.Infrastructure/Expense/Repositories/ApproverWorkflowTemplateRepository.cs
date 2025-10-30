using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Expense.Repositories;
using ThaiTuanERP2025.Infrastructure.Common.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Expense.Repositories
{
	public class ApprovalWorkflowTemplateRepository : BaseRepository<ApprovalWorkflowTemplate>, IApprovalWorkflowTemplateRepository
	{
		public ApprovalWorkflowTemplateRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider configurationProvider) : base(dbContext, configurationProvider)
		{
		}

		public async Task<bool> ExistsActiveForScopeAsync(string documentType, CancellationToken cancellationToken = default) {
			var query = _dbSet.AsNoTracking()
				.Where(x => !x.IsDeleted && x.IsActive && x.DocumentType == documentType);

			return await query.AnyAsync();
		}
		
		public async Task<List<ApprovalWorkflowTemplate>> ListByFilterAsync(string? documentType, bool? isActive, CancellationToken cancellationToken = default) {
			var query = _dbSet.AsNoTracking().Where(x => !x.IsDeleted);

			if (!string.IsNullOrWhiteSpace(documentType))
				query = query.Where(x => x.DocumentType == documentType);

			if (isActive.HasValue)
				query = query.Where(x => x.IsActive == isActive.Value);

			return await query.OrderByDescending(x => x.IsActive)
				      .ThenBy(x => x.DocumentType)
				      .ThenBy(x => x.Name)
				      .ToListAsync(cancellationToken);
		}
	}
}
