using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Repositories
{
	public  interface IApprovalFlowInstanceRepository : IBaseRepository<ApprovalFlowInstance>
	{
		/// <summary>
		/// Lấy 1 StepInstance theo Id, kèm FlowInstance (để có trạng thái flow, CompletedAt, v.v.)
		/// Trả về entity được tracking (asNoTracking = false) để handler có thể cập nhật trực tiếp.
		/// </summary>
		Task<ApprovalStepInstance?> GetStepWithFlowAsync(Guid stepId, CancellationToken cancellationToken = default);

		/// Lấy bước Pending kế tiếp theo OrderIndex trong 1 flow (tracking để cập nhật ngay).
		Task<ApprovalStepInstance?> GetNextPendingStepAsync(Guid flowInstanceId, int currentOrderIndex, CancellationToken ct = default);
	}
}
