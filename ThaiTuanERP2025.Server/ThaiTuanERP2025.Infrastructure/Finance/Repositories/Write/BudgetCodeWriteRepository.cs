using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Application.Finance.BudgetCodes;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Domain.Finance.Repositories;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Finance.Repositories
{
	public class BudgetCodeWriteRepository : BaseWriteRepository<BudgetCode>, IBudgetCodeWriteRepository {
		private readonly ThaiTuanERP2025DbContext _db;
		public BudgetCodeWriteRepository(ThaiTuanERP2025DbContext dbContext, IConfigurationProvider configurationProvider) : base(dbContext, configurationProvider)
		{
			_db = dbContext;
		}
		
		public async Task<List<BudgetCodeWithAmountDto>> GetWithAmountForPeriodAsync(int year, int month, Guid departmentId, CancellationToken cancellationToken) {
			// 1 ) PeriodId
			var periodId = await _db.BudgetPeriods
				.Where(p => p.Year == year && p.Month == month)
				.Select(p => (Guid?)p.Id)
				.SingleOrDefaultAsync(cancellationToken);
			if (periodId is null)
				return new List<BudgetCodeWithAmountDto>();

			// 2 )  INNER JOIN + ORDER trước khi map DTO
			var baseQuery =
				from bc in _db.BudgetCodes
				join bg in _db.BudgetGroups on bc.BudgetGroupId equals bg.Id
				join bp in _db.BudgetPlans.Where(x => x.BudgetPeriodId == periodId.Value && (x.DepartmentId == departmentId))
					on bc.Id equals bp.BudgetCodeId
				select new
				{
					bc.Id,
					bc.Code,
					bc.Name,
					BudgetPlanId = (Guid?)bp.Id,
					Amount = (decimal?)bp.Amount,
					BudgetGroupName = bg.Name
				};
			
			var ordered = baseQuery.OrderBy(x => x.Code);

			var result = await ordered.Select(x => new BudgetCodeWithAmountDto(
				x.Id, x.Code, x.Name, year, month, x.BudgetPlanId, x.Amount, x.BudgetGroupName
			)).AsNoTracking().ToListAsync(cancellationToken);

			return result;
		}
	}
}
