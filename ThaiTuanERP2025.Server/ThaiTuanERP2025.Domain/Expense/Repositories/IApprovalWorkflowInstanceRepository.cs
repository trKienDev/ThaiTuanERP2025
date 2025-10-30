﻿using ThaiTuanERP2025.Domain.Common.Repositories;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Domain.Expense.Repositories
{
	public interface IApprovalWorkflowInstanceRepository : IBaseRepository<ApprovalWorkflowInstance>
	{
		Task<List<ApprovalWorkflowInstance>> ListByFilterAsync(
			string? documentType, Guid? documentId, WorkflowStatus? status,
			string? budgetCode, decimal? minAmount, decimal? maxAmount,
			CancellationToken cancellationToken
		);
	}
}
