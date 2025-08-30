using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Application.Expense.Repositories;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Infrastructure.Common;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Approval.Repositories
{
	public sealed class ApprovalActionRepository : BaseRepository<ApprovalAction>, IApprovalActionRepository
	{
		public ApprovalActionRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider mapper) 
			: base(dbContext, mapper) { }

		public async Task<IReadOnlyList<ApprovalAction>> ListByStepAsync(Guid stepInstanceId, CancellationToken cancellationToken = default) {
			return await Query(asNoTracking: true)
				.Where(a => a.StepInstanceId == stepInstanceId)
				.OrderBy(a => a.OccuredAt)
				.ToListAsync(cancellationToken);
		}
	}
}
