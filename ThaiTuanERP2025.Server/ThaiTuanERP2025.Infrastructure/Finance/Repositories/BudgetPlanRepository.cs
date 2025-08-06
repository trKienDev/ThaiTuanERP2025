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
	public class BudgetPlanRepository : BaseRepository<BudgetPlan>, IBudgetPlanRepository
	{
		public BudgetPlanRepository(ThaiTuanERP2025DbContext dbContext) : base(dbContext)
		{
		}
		// Implement custom methods here
	}
}
