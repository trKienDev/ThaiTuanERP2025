using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Expense.Repositories;
using ThaiTuanERP2025.Infrastructure.Common.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Expense.Repositories
{
	public class ExpensePaymentRepository : BaseWriteRepository<ExpensePayment>, IExpensePaymentRepository
	{
		public ExpensePaymentRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider configurationProvider) : base(dbContext, configurationProvider)
		{
		}

		public async Task<ExpensePayment?> GetDetailByIdAsync(Guid id, CancellationToken cancellationToken = default)
		{
			return await _dbSet
				.AsNoTracking()
				.AsSplitQuery()
				.Include(p => p.CreatedByUser).ThenInclude(u => u.Department)
				.Include(p => p.Supplier)
				.Include(p => p.Items).ThenInclude(i => i.BudgetCode)
				.Include(p => p.Items).ThenInclude(i => i.CashoutCode)
				.Include(p => p.Items).ThenInclude(i => i.Invoice!).ThenInclude(inv => inv.Files).ThenInclude(f => f.File)
				.Include(p => p.Items).ThenInclude(i => i.Invoice!).ThenInclude(inv => inv.CreatedByUser)
				.Include(p => p.Attachments)
				.SingleOrDefaultAsync(p => p.Id == id && !p.IsDeleted, cancellationToken);
		}



		public async Task<ExpenseWorkflowInstance?> GetWorkflowInstanceAsync(Guid documentId, CancellationToken cancellationToken = default)
		{
			return await _context.Set<ExpenseWorkflowInstance>()
				.AsNoTracking()
				.AsSplitQuery()
				.Include(i => i.Steps)
				.SingleOrDefaultAsync(i =>
					i.DocumentType == "ExpensePayment" &&
					i.DocumentId == documentId,
					cancellationToken
				);
		}
	}
}
