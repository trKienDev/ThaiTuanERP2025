using ThaiTuanERP2025.Domain.Common.Repositories;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Repositories
{
	public interface IApprovalWorkflowTemplateRepository : IBaseRepository<ApprovalWorkflowTemplate>
	{
		Task<bool> ExistsActiveForScopeAsync(string documentType, CancellationToken cancellationToken = default);
		Task<List<ApprovalWorkflowTemplate>> ListByFilterAsync(string? documentType, bool? isActive, CancellationToken cancellationToken = default);
	}
}
