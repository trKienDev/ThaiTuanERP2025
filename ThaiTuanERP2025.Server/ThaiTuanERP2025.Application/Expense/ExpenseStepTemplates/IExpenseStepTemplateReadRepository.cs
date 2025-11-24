using ThaiTuanERP2025.Application.Expense.ExpenseStepTemplates.Contracts;
using ThaiTuanERP2025.Application.Shared.Repositories;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.ExpenseStepTemplates
{
	public interface IExpenseStepTemplateReadRepository : IBaseReadRepository<ExpenseStepTemplate, ExpenseStepTemplateDto>
	{
	}
}
