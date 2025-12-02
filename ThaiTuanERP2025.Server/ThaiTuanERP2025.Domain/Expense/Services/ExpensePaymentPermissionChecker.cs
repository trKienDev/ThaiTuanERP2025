using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Domain.Expense.Services
{
	public static class ExpensePaymentPermissionChecker
	{
		public static bool CanViewExpensePayment(ExpensePayment expensePayment, Guid userId)
		{
			if (expensePayment.CreatedByUserId == userId)
				return true;

			if (expensePayment.ManagerApproverId == userId)
				return true;

			// Follower
			// if (expensePayment.Followers.Any(f => f.UserId == userId)) return true;

			var wf = expensePayment.CurrentWorkflowInstance;
			if (wf == null) return false;

			foreach (var step in wf.Steps)
			{
				var approverIds = step.GetResolvedApproverIds();
				// Là approver của step?
				if (approverIds.Contains(userId))
					return true;

				// Đã approve?
				if (step.ApprovedBy == userId)
					return true;

				// Đã reject?
				if (step.RejectedBy == userId)
					return true;
			}

			return false;
		}
	}

}
