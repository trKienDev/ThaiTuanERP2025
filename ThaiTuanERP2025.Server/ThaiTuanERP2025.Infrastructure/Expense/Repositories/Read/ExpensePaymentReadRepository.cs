using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Application.Expense.ExpensePayments.Contracts;
using ThaiTuanERP2025.Application.Expense.ExpensePayments.Repositories;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;

namespace ThaiTuanERP2025.Infrastructure.Expense.Repositories.Read
{
	public sealed class ExpensePaymentReadRepository : BaseReadRepository<ExpensePayment, ExpensePaymentDto>, IExpensePaymentReadRepository
	{
		public ExpensePaymentReadRepository(ThaiTuanERP2025DbContext dbContext, IMapper mapperConfig) : base(dbContext, mapperConfig) { }

		public async Task<string?> GetNameAsync(Guid expensePaymentId, CancellationToken cancellationToken = default)
		{
			return await _dbSet.AsNoTracking()
				.Where(x => x.Id == expensePaymentId)
				.Select(x => x.Name)
				.SingleOrDefaultAsync(cancellationToken);
		}

		public async Task<Guid?> GetCreatorIdAsync(Guid expensePaymentId, CancellationToken cancellationToken = default)
		{
			return await _dbSet.AsNoTracking()
				.Where(x => x.Id == expensePaymentId)
				.Select(x => x.CreatedByUserId)
				.SingleOrDefaultAsync(cancellationToken);
		}

		public async Task<Guid> GetManagerApproverId(Guid expensePaymentId, CancellationToken cancellationToken = default)
		{
			return await _dbSet.AsNoTracking()
				.Where(x => x.Id == expensePaymentId)
				.Select(x => x.ManagerApproverId)
				.SingleOrDefaultAsync(cancellationToken);
		}

		public async Task<ExpensePaymentLookupDto?> GetLookupById(Guid id, CancellationToken cancellationToken = default)
		{
			return await _dbSet.AsNoTracking()
				.Where(x => x.Id == id)
				.ProjectTo<ExpensePaymentLookupDto>(_mapperConfig)
				.SingleOrDefaultAsync(cancellationToken);
		}

		public async Task<ExpensePaymentDetailDto?> GetDetailById(Guid id, CancellationToken cancellationToken = default)
		{
			var payment = await _dbSet
				.Include(x => x.CreatedByUser)
				.Include(x => x.Supplier)
				.Include(x => x.CurrentWorkflowInstance).ThenInclude(w => w.Steps).ThenInclude(s => s.ApprovedByUser)
				.Include(x => x.CurrentWorkflowInstance).ThenInclude(w => w.Steps).ThenInclude(s => s.RejectedByUser)
				.Include(x => x.Items).ThenInclude(i => i.BudgetPlanDetail).ThenInclude(b => b.BudgetCode)
				.Include(x => x.Items).ThenInclude(i => i.InvoiceFile)
				.Include(x => x.Attachments).ThenInclude(a => a.StoredFile)
                                .FirstOrDefaultAsync(x => x.Id == id);

			return _mapper.Map<ExpensePaymentDetailDto>(payment);
		}

		public async Task<ExpensePayment?> GetByInvoiceFileIdAsync(Guid fileId, CancellationToken cancellationToken = default)
		{
			return await _dbSet
				.Include(x => x.Items)
				.Include(x => x.CurrentWorkflowInstance).ThenInclude(w => w.Steps)
				.FirstOrDefaultAsync(x => x.Items.Any(i => i.InvoiceFileId == fileId), cancellationToken);
		}
	}
}
