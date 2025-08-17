using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Partner.Repositories;
using ThaiTuanERP2025.Domain.Partner.Entities;
using ThaiTuanERP2025.Infrastructure.Common;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Partner.Repositories
{
	public class PartnerBankAccountRepository : BaseRepository<PartnerBankAccount>, IPartnerBankAccountRepository
	{
		public PartnerBankAccountRepository(ThaiTuanERP2025DbContext dbContext) : base(dbContext) { }

		public Task<PartnerBankAccount?> GetBySupplierIdAsync(Guid supplierId, CancellationToken cancellationToken = default) {
			return _dbSet.FirstOrDefaultAsync(x => x.SupplierId == supplierId, cancellationToken);
		}

		public Task<bool> ExistsForSupplierAsync(Guid supplierId, CancellationToken cancellationToken = default) {
			return _dbSet.AnyAsync(x => x.SupplierId == supplierId, cancellationToken);
		}
	}
}
