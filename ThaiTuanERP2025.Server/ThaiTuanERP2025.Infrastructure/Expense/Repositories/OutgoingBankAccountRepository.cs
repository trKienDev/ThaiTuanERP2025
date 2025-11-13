using AutoMapper;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Expense.Repositories;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Expense.Repositories
{
	public sealed class OutgoingBankAccountRepository : BaseWriteRepository<OutgoingBankAccount>, IOutgoingBankAccountRepository
	{
		public OutgoingBankAccountRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider configurationProvider)
			: base(dbContext, configurationProvider) { }
	}
}
