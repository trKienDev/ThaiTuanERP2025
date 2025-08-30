using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Repositories
{
	public interface IApprovalActionRepository : IBaseRepository<ApprovalAction>
	{
		Task<IReadOnlyList<ApprovalAction>> ListByStepAsync(Guid stepInstanceId, CancellationToken cancellationToken);
	}
}
