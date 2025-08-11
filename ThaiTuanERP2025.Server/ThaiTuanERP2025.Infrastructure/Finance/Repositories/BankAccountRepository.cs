using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Models;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Application.Finance.Repositories;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Infrastructure.Common;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Finance.Repositories
{
	public class BankAccountRepository : BaseRepository<BankAccount>, IBankAccountRepository 
	{
		public BankAccountRepository(ThaiTuanERP2025DbContext dbContext) : base(dbContext) {}

		public Task<bool> ExistsDuplicateAsync(string accounNumber, string bankName, Guid? departmentId, string? customerName, Guid? excludeId = null, CancellationToken cancellationToken = default) {
			var q = _dbSet.AsNoTracking().Where(x =>
				x.AccountNumber == accounNumber &&
				x.BankName == bankName &&
				x.DepartmentId == departmentId &&
				x.CustomerName == customerName
			);

			if (excludeId.HasValue) q = q.Where(x => x.Id != excludeId.Value);
			return q.AnyAsync(cancellationToken);
		}
	}
}
