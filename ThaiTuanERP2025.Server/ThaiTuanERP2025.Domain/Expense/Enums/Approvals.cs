using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Domain.Expense.Enums
{
	public enum ApprovalStepMode { Serial = 0, Parrallel = 1 }
	public enum ApprovalFlowStatus { Draft, Pending, InProgress, Approved, Rejected, Canceled }
	public enum ApprovalStepStatus { Pending, InProgress, Approved, Rejected, Skipped }
	public enum ApprovalActionType { Approve, Reject, Comment }
	public enum ApprovalResolverType {  FixedUser = 1, UserPickFromList = 2, UserList = 3 }	
}
