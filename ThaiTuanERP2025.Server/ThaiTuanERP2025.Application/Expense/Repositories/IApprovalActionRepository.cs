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


		/// Lấy toàn bộ actions theo danh sách stepIds (để query 1 lần)
		Task<IReadOnlyList<ApprovalAction>> ListByStepIdsAsync(IEnumerable<Guid> stepIds, CancellationToken cancellationToken = default);
	}
}
