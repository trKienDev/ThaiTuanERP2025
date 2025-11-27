using AutoMapper;
using ThaiTuanERP2025.Application.Expense.ExpenseWorkflows.Contracts;
using ThaiTuanERP2025.Application.Expense.ExpenseWorkflows.Repositories;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;

namespace ThaiTuanERP2025.Infrastructure.Expense.Repositories.Read
{
	public sealed class ExpenseWorkflowInstanceReadRepository : BaseReadRepository<ExpenseWorkflowInstance, ExpenseWorkflowInstanceDto>, IExpenseWorkflowInstanceReadRepository
	{
		public ExpenseWorkflowInstanceReadRepository(ThaiTuanERP2025DbContext dbContext, IMapper mapperConfig) : base(dbContext, mapperConfig) { }
	}
}
