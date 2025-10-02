using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Application.Expense.Repositories;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Infrastructure.Common;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Expense.Repositories
{
	public sealed class BankAccountRepository : BaseRepository<BankAccount>, IBankAccountRepository
	{
		public BankAccountRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider configurationProvider)
			: base(dbContext, configurationProvider) { }

		public Task<bool> ExistsForUserAsync(Guid userId, CancellationToken cancellationToken) {
			return _dbSet.AnyAsync(x => x.UserId == userId, cancellationToken);
		}

		public Task<BankAccount?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken) {
			return _dbSet.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
		}

		public async Task<IReadOnlyList<BankAccount>> ListBySupplierIdAsync(Guid supplierId, CancellationToken cancellationToken) {
			return await _dbSet.Where(x => x.SupplierId == supplierId)
				.OrderBy(x => x.BeneficiaryName).ThenBy(x => x.BankName)
				.ToListAsync();
		}
	}
}
