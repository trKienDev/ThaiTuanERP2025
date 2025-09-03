using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Application.Expense.Requests
{
	public sealed record CreateFlowStepRequest(
		string Title,
		int Order,
		List<Guid> CandidateUserIds,
		ApprovalStepFlowType FlowType, 
		int? SlaHours = null,
		string? Description = null
	);

	public sealed record CreateApprovalWorkflowRequest(
		string Name,
		bool IsActive,
		List<CreateFlowStepRequest> Steps
	);
}
