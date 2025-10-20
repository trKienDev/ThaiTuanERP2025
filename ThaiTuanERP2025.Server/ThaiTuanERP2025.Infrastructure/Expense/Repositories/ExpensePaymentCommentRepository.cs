using AutoMapper;
using ThaiTuanERP2025.Application.Expense.Repositories;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Infrastructure.Common;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Expense.Repositories
{
	public class ExpensePaymentCommentRepository : BaseRepository<ExpensePaymentComment>, IExpensePaymentCommentRepository
	{
		public ExpensePaymentCommentRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider configurationProvider) : base(dbContext, configurationProvider)
		{
		}
	}
}
