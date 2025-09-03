using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Application.Expense.Requests
{
	public sealed record UdpateApprovalWorkflowRequest(
		string Title,
		int Order,
		ApprovalStepFlowType FlowType,
		int? SlaHours, 
		IReadOnlyList<Guid> CandidateUserIds,
		string? Description = null
	);

	public sealed record UpdateApprovalWorkflowRequest(
		string Name, 
		bool? IsActive,
		IReadOnlyList<UdpateApprovalWorkflowRequest> Steps
	);
}
