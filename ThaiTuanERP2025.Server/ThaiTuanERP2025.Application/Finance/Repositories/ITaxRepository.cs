using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Common.Models;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.Repositories
{
	public interface ITaxRepository : IBaseRepository<Tax>
	{
		Task<bool> PolicyNameExistsAsync(string policyName, Guid? excludeId = null, CancellationToken cancellationToken = default);
		Task<List<TaxDto>> ListTaxDtosAsync(bool? isActive, string? search, CancellationToken cancellationToken);
		Task<TaxDto?> GetTaxDtoByIdAsync(Guid id, CancellationToken cancellationToken);
		Task<TaxDto?> GetTaxDtoByNameAsync(string policyName, CancellationToken cancellationToken);
		Task<PagedResult<TaxDto>> GetPagedDtosAsync(PagedRequest request, CancellationToken cancellationToken);
	}
}
