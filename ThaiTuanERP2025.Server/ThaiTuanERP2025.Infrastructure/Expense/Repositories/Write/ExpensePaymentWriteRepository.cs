using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Expense.Repositories;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Expense.Repositories.Write
{
	public class ExpensePaymentWriteRepository : BaseWriteRepository<ExpensePayment>, IExpensePaymentWriteRepository
	{
		public ExpensePaymentWriteRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider configurationProvider) : base(dbContext, configurationProvider)
		{
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
