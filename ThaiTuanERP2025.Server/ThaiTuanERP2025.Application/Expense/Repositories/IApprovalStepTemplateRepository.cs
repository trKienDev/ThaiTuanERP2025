using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Repositories
{
	public interface IApprovalStepTemplateRepository : IBaseRepository<ApprovalStepTemplate>
	{
		Task<bool> ExistOrderAsync(Guid workflowTemplateId, int order, Guid? excludeId = null, CancellationToken cancellationToken = default);
	}
}
