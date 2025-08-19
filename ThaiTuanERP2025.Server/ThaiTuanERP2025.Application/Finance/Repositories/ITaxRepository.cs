using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.Repositories
{
	public interface ITaxRepository : IBaseRepository<Tax>
	{
		Task<bool> PolicyNameExistsAsync(string policyName, Guid? excludeId = null, CancellationToken cancellationToken = default);
	}
}
