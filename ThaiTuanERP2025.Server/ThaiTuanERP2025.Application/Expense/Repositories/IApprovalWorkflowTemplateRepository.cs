using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Repositories
{
	public interface IApprovalWorkflowTemplateRepository : IBaseRepository<ApprovalWorkflowTemplate>
	{
		Task<bool> ExistsActiveForScopeAsync(string documentType, CancellationToken cancellationToken = default);
		Task<List<ApprovalWorkflowTemplate>> ListByFilterAsync(string? documentType, bool? isActive, CancellationToken cancellationToken = default);
	}
}
