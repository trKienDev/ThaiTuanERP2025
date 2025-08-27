using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Models;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Application.Finance.Repositories;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Infrastructure.Common;
using ThaiTuanERP2025.Infrastructure.Common.Querying;
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

		public async Task<List<TaxDto>> ListTaxDtosAsync(bool? isActive, string? search, CancellationToken cancellationToken) {
			var query = _dbSet.AsNoTracking();

			if (isActive.HasValue)
				query = query.Where(tax => tax.IsActive == isActive.Value);

			if (!string.IsNullOrWhiteSpace(search))
			{
				var s = search.Trim();
				query = query.Where(tax => EF.Functions.Like(tax.PolicyName, $"%{s}%")
					      || EF.Functions.Like(tax.Description!, $"%{s}%"));
			}

			return await query.OrderBy(tax => tax.PolicyName)
				.Select(tax => new TaxDto(
					tax.Id,
					tax.PolicyName,
					tax.Rate,
					tax.PostingLedgerAccountId,
					tax.PostingLedgerAccount.Number,
					tax.PostingLedgerAccount.Name,
					tax.Description,
					tax.IsActive
				)).ToListAsync(cancellationToken);
		}

		public async Task<TaxDto?> GetTaxDtoByIdAsync(Guid id, CancellationToken cancellationToken)
		{
			return await _dbSet.AsNoTracking()
				.Where(tax => tax.Id == id)
				.Select(tax => new TaxDto(
					tax.Id,
					tax.PolicyName,
					tax.Rate,
					tax.PostingLedgerAccountId,
					tax.PostingLedgerAccount.Number,
					tax.PostingLedgerAccount.Name,
					tax.Description,
					tax.IsActive
				)).SingleOrDefaultAsync(cancellationToken);
		}
		
		public async Task<TaxDto?> GetTaxDtoByNameAsync(string policyName, CancellationToken cancellationToken) {
			return await _dbSet.AsNoTracking()	
				.Where(tax => tax.PolicyName == policyName)
				.Select(t => new TaxDto(
					t.Id,
					t.PolicyName,
					t.Rate,
					t.PostingLedgerAccountId,
					t.PostingLedgerAccount.Number,
					t.PostingLedgerAccount.Name,
					t.Description,
					t.IsActive
				)).SingleOrDefaultAsync(cancellationToken);
		}

		public async Task<PagedResult<TaxDto>> GetPagedDtosAsync(PagedRequest request, CancellationToken cancellationToken)
		{
			var query = _dbSet.AsNoTracking();

			// filters
			if (request.Filters != null && request.Filters.TryGetValue("isActive", out var isActiveStr) && bool.TryParse(isActiveStr, out var isActive))
			{
				query = query.Where(t => t.IsActive == isActive);
			}

			// Keyword
			var keyword = KeywordFilter.Normalize(request.Keyword);
			if (!string.IsNullOrWhiteSpace(keyword))
			{
				query = query.Where(t =>
					EF.Functions.Like(t.PolicyName, $"%{keyword}%") ||
					EF.Functions.Like(t.Description!, $"%{keyword}%") ||
					EF.Functions.Like(t.PostingLedgerAccount.Number, $"%{keyword}%") ||
					EF.Functions.Like(t.PostingLedgerAccount.Name, $"%{keyword}%")
				);
			}

			// Sort map
			var sortMap = new Dictionary<string, Expression<Func<Tax, object>>>(StringComparer.OrdinalIgnoreCase)
			{
				["policyname"] = t => t.PolicyName,
				["rate"] = t => t.Rate,
				["account"] = t => t.PostingLedgerAccount.Number
			};

			var ordered = query.ApplySorting(request.Sort, sortMap, defaultField: "policyname");

			// Projection + paging
			return await ordered.ToPagedResultAsync(
				request.PageIndex, request.PageSize,
				t => new TaxDto(
					t.Id,
					t.PolicyName,
					t.Rate,
					t.PostingLedgerAccountId,
					t.PostingLedgerAccount.Number,
					t.PostingLedgerAccount.Name,
					t.Description,
					t.IsActive
				),
				cancellationToken
			);
		}
	}
}
