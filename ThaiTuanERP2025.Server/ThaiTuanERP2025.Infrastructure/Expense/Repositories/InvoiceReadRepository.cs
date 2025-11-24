using AutoMapper;
using ThaiTuanERP2025.Application.Expense.Invoices;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Expense.Repositories
{
	public class InvoiceReadRepository : BaseWriteRepository<Invoice>, IInvoiceReadRepository
	{
		private readonly IConfigurationProvider _mapperConfig;
		public InvoiceReadRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider configurationProvider)
			: base(dbContext, configurationProvider)
		{
			_mapperConfig = configurationProvider;
		}
	}
}
