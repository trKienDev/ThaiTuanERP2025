using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalWorkflow.SubmitApproval
{
	public sealed record SubmitApprovalCommand(
		string DocumentType, Guid DocumentId, 
		Guid? SelectedApproverId // dùng cho UserPickFromList
	) : IRequest<Guid>;
}
