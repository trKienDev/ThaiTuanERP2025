using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Partner.Entities;

namespace ThaiTuanERP2025.Application.Partner.Repositories
{
	public interface IPartnerBankAccountRepository : IBaseRepository<PartnerBankAccount>
	{
		Task<PartnerBankAccount?> GetBySupplierIdAsync(Guid supplierId, CancellationToken ct = default);
		Task<bool> ExistsForSupplierAsync(Guid supplierId, CancellationToken ct = default);
	}
}
