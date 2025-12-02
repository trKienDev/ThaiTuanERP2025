using AutoMapper;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Expense.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;

namespace ThaiTuanERP2025.Infrastructure.Expense.Repositories.Write
{
	public sealed class ExpensePaymentAttachmentWriteRepository : BaseWriteRepository<ExpensePaymentAttachment>, IExpensePaymentAttachmentWriteRepository
	{
		public ExpensePaymentAttachmentWriteRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider mapperConfig) : base(dbContext, mapperConfig)
		{
		}
	}
}
