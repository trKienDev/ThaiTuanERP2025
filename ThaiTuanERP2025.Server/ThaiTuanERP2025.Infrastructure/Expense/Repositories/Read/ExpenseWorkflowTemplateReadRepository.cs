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

		public async Task<ExpenseWorkflowTemplateDto?> GetDetailByIdAsync(Guid id, CancellationToken cancellationToken = default)
		{
			var entity = await _dbSet.AsNoTracking()
				.Include(x => x.Steps)
				.Where(x => x.Id == id)
				.SingleOrDefaultAsync(cancellationToken);

			if (entity is null)
				return null;

			// Map root + steps (steps sẽ được AfterMap xử lý ApproverIds)
			var dto = _mapper.Map<ExpenseWorkflowTemplateDto>(entity);

			// Sort steps sau khi map
			dto.Steps = dto.Steps
				.OrderBy(s => s.Order)
				.ToList();

			return dto;
		}
	}
}
	