using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Expense.Repositories;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Expense.Repositories
{
	public class InvoiceFileRepository : BaseWriteRepository<InvoiceFile>, IInvoiceFileRepository
	{
		public InvoiceFileRepository(ThaiTuanERP2025DbContext context, IConfigurationProvider configurationProvider)
			: base(context, configurationProvider) {}
	}
}
