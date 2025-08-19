using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Finance.Repositories;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Infrastructure.Common;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Finance.Repositories
{
	public class TaxRepository : BaseRepository<Tax>, ITaxRepository
	{
		public TaxRepository(ThaiTuanERP2025DbContext dbContext) : base(dbContext) { }

		public Task<bool> PolicyNameExistsAsync(string policyName, Guid? excludeId = null, CancellationToken cancellationToken = default)
		{
			var query = _dbSet.AsNoTracking().Where(x => x.PolicyName == policyName);
			if (excludeId.HasValue) query = query.Where(x => x.Id != excludeId.Value);
			return query.AnyAsync(cancellationToken);
		}
	}
}
