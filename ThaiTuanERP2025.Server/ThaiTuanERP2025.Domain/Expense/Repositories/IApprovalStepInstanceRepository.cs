using ThaiTuanERP2025.Domain.Common.Repositories;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Repositories
{
	public interface IApprovalStepInstanceRepository : IBaseRepository<ApprovalStepInstance>
	{
		Task<ApprovalStepInstance?> GetByIdWithWorkflowAsync(Guid stepId, CancellationToken cancellationToken = default);
		Task<ApprovalStepInstance?> GetCurrentStepAsync(Guid workflowId, CancellationToken cancellationToken = default);
	}
}
