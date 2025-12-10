using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Shared.Repositories;
using ThaiTuanERP2025.Domain.Finance.Entities;

namespace ThaiTuanERP2025.Application.Finance.BudgetGroups
{
	public interface IBudgetGroupReadRepository : IBaseReadRepository<BudgetGroup, BudgetGroupDto> {

	}
}
