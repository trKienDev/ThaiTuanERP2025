using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Expense.Repositories;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Expense.Repositories.Write
{
	public sealed class SupplierWriteRepository : BaseWriteRepository<Supplier>, ISupplierWriteRepository
	{
		public SupplierWriteRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider configurationProvider) : base(dbContext, configurationProvider) { }
	}
}
