using ThaiTuanERP2025.Application.Expense.ExpenseWorkflowTemplates.Contracts;
using ThaiTuanERP2025.Application.Shared.Repositories;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.ExpenseWorkflowTemplates
{
	public interface IExpenseWorkflowTemplateReadRepository : IBaseReadRepository<ExpenseWorkflowTemplate, ExpenseWorkflowTemplateDto>
	{
		Task<ExpenseWorkflowTemplateDto?> GetDetailByIdAsync(Guid id, CancellationToken cancellationToken = default);
	}
}
