using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Expense.Repositories;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Expense.Enums;
using ThaiTuanERP2025.Infrastructure.Common;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Approval.Repositories
{
	public class ApprovalFlowInstanceRepository : BaseRepository<ApprovalFlowInstance>, IApprovalFlowInstanceRepository 
	{
		public ApprovalFlowInstanceRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider mapper) 
			: base(dbContext, mapper) { }

		public async Task<ApprovalStepInstance?> GetStepWithFlowAsync(Guid stepId, CancellationToken cancellationToken = default) {
			return await Query(asNoTracking: false)
				.SelectMany(fi => fi.Steps)
				.Where(si => si.Id == stepId)
				.Include(si => si.FlowInstance)  // cần FlowInstance để set Approved/Rejected khi kết thúc
				.FirstOrDefaultAsync(cancellationToken);
		}

		public async Task<ApprovalStepInstance?> GetNextPendingStepAsync(Guid flowInstanceId, int currentOrderIndex, CancellationToken cancellationToken = default) {
			return await Query(asNoTracking: false)
				.Where(fi => fi.Id == flowInstanceId)
				.SelectMany(fi => fi.Steps)	
				.Where(si => si.Status == ApprovalStepStatus.Pending && si.OrderIndex > currentOrderIndex)
				.OrderBy(si => si.OrderIndex)
				.FirstOrDefaultAsync(cancellationToken);
		}
 	}
}
