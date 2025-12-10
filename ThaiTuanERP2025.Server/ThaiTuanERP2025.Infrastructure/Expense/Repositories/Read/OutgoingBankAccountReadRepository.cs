using ThaiTuanERP2025.Application.Expense.OutgoingBankAccounts;
using ThaiTuanERP2025.Application.Expense.OutgoingBankAccounts.Contracts;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;

namespace ThaiTuanERP2025.Infrastructure.Expense.Repositories.Read
{
	public sealed class OutgoingBankAccountReadRepository : BaseReadRepository<OutgoingBankAccount, OutgoingBankAccountDto>, IOutgoingBankAccountReadRepository	
	{
		public OutgoingBankAccountReadRepository(ThaiTuanERP2025DbContext dbContext, AutoMapper.IMapper mapperConfig) : base(dbContext, mapperConfig) { }
	}
}
