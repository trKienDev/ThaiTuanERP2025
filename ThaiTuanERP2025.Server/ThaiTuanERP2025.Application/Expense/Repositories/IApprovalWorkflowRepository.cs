using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Repositories
{
	public interface IApprovalWorkflowRepository : IBaseRepository<ApprovalWorkflow>
	{
		Task<ApprovalWorkflow?> SingleOrDefaultIncludingAsync(Guid id, CancellationToken cancellationToken = default);
		Task<ApprovalWorkflowDto> AddAndReturnDtoAsync(ApprovalWorkflow entity, CancellationToken cancellationToken = default);
	}
}
