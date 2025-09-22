using ThaiTuanERP2025.Application.Common.Utils;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Application.Expense.Services.ApprovalWorkflows
{
	public static class StepGuards
	{
		public static void EnsureWorkflowMatches(ApprovalStepInstance step, Guid workflowId) {
			if (step.WorkflowInstanceId != workflowId)
				throw new ConflictException("Step không thuộc workflow này");
		}

		public static void EnsureWaiting(ApprovalStepInstance step) {
			if(step.Status != StepStatus.Waiting)
				throw new ConflictException("Step không ở trạng thái chờ duyệt");
		}

		public static void EnsureApproverAuthorized(ApprovalStepInstance step, Guid currentUserId) {
			// Nếu đã lock SelectedApproverId → user phải trùng
			if (step.SelectedApproverId.HasValue && step.SelectedApproverId.Value != currentUserId)
				throw new UnauthorizedAccessException("Bạn không phải người được chỉ định để duyệt bước này.");

			// Nếu chưa lock hoặc one-of-n → kiểm tra nằm trong candidates
			var candidates = JsonGuidArray.Parse(step.ResolvedApproverCandidatesJson);
			if (candidates.Length > 0 && !candidates.Contains(currentUserId))
				throw new UnauthorizedAccessException("Bạn không nằm trong danh sách ứng viên duyệt bước này.");
		}
	}
}
