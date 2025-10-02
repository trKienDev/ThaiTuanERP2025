using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Application.Common.Models;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Application.Expense.Repositories;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Infrastructure.Common;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Expense.Repositories
{
	public class InvoiceRepository : BaseRepository<Invoice>, IInvoiceRepository {
		private readonly IConfigurationProvider _mapperConfig;
		public InvoiceRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider configurationProvider) 
			: base(dbContext, configurationProvider) {
			_mapperConfig = configurationProvider;	
		}

		public async Task<PagedResult<InvoiceDto>> GetInvoicesPagedAsync(int page, int  pageSize, string? keyword, CancellationToken cancellationToken) {
			if (page <= 0)
				page = 1;
			if(pageSize <= 0) 
				pageSize = 20;

			var query = Query();
			if (!string.IsNullOrWhiteSpace(keyword))
			{
				var kw = keyword.Trim().ToLower();
				query = query.Where(i => i.InvoiceName.Contains(kw) || i.InvoiceNumber.Contains(kw));
			}

			var total = await query.CountAsync(cancellationToken);
			var items = await query.OrderByDescending(i => i.IssueDate)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ProjectTo<InvoiceDto>(_mapperConfig)
				.ToListAsync();

			return new PagedResult<InvoiceDto>(items, total, page, pageSize);
		}

		public async Task<PagedResult<InvoiceDto>> GetByCreatorPagedAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken) {
			if (page <= 0) page = 1;
			if (pageSize <= 0) pageSize = 20;

			var query = Query().Where(i => i.CreatedByUserId == userId);
			var total = await query.CountAsync(cancellationToken);
			var items = await query.OrderByDescending(i => i.IssueDate)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ProjectTo<InvoiceDto>(_mapperConfig)
				.ToListAsync(cancellationToken);

			return new PagedResult<InvoiceDto>(items, total, page, pageSize);
		}
	}
}
