using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Models;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Application.Finance.Repositories;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Finance.Repositories
{
	public class BankAccountReadRepository : IBankAccountReadRepository
	{
		private readonly ThaiTuanERP2025DbContext _dbContext;
		private readonly IConfigurationProvider _mapperConfiguration;
		public BankAccountReadRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider mapperConfiguration)
		{
			_dbContext = dbContext;
			_mapperConfiguration = mapperConfiguration;
		}

		public async Task<BankAccountDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
		{
			return await _dbContext.Set<BankAccount>().AsNoTracking()
				.Where(x => x.Id == id)
				.ProjectTo<BankAccountDto>(_mapperConfiguration)
				.FirstOrDefaultAsync(cancellationToken);
		}
		public async Task<PagedResult<BankAccountDto>> SearchPagedAsync(bool? onlyActive, Guid? departmentId, int page, int pageSize, CancellationToken cancellationToken = default)
		{
			var query = _dbContext.Set<BankAccount>().AsNoTracking();

			if (onlyActive is true)
				query = query.Where(x => x.IsActive);
			if (departmentId.HasValue)
				query = query.Where(x => x.DepartmentId == departmentId);

			var total = await query.CountAsync(cancellationToken);
			var items = await query.OrderBy(x => x.BankName).ThenBy(x => x.AccountNumber)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ProjectTo<BankAccountDto>(_mapperConfiguration)
				.ToListAsync(cancellationToken);

			return new PagedResult<BankAccountDto>(items, total, page, pageSize);
		}
	}
}
