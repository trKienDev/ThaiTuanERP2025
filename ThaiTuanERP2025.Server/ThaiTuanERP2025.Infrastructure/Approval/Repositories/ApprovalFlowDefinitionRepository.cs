using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Expense.Repositories;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Infrastructure.Common;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Approval.Repositories
{
	public class ApprovalFlowDefinitionRepository : BaseRepository<ApprovalFlowDefinition>, IApprovalFlowDefinitionRepository
	{
		public ApprovalFlowDefinitionRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider mapper)
			: base(dbContext, mapper) { }

		public async Task<ApprovalFlowDefinition?> GetLastestActiveByDocumentTypeAsync(string documentType, CancellationToken cancellationToken) {
			var flowDefinition = await _dbSet.Where(x => x.DocumentType == documentType && x.IsActive)
				.OrderByDescending(x => x.Version)
				.FirstOrDefaultAsync(cancellationToken);

			if (flowDefinition == null)
				return null;

			await _dbSet.Entry(flowDefinition).Collection(d => d.Steps)
				.Query().OrderBy(s => s.OrderIndex).LoadAsync(cancellationToken);

			return flowDefinition;
		}
	}
}
