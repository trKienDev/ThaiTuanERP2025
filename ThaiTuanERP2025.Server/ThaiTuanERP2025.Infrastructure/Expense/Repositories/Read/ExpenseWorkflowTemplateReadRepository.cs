using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Application.Expense.ExpenseWorkflowTemplates;
using ThaiTuanERP2025.Application.Expense.ExpenseWorkflowTemplates.Contracts;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;

namespace ThaiTuanERP2025.Infrastructure.Expense.Repositories.Read
{
	public sealed class ExpenseWorkflowTemplateReadRepository : BaseReadRepository<ExpenseWorkflowTemplate, ExpenseWorkflowTemplateDto>, IExpenseWorkflowTemplateReadRepository
	{
		public ExpenseWorkflowTemplateReadRepository(ThaiTuanERP2025DbContext dbContext, IMapper mapperConfig) : base(dbContext, mapperConfig) { }
        }
}
	