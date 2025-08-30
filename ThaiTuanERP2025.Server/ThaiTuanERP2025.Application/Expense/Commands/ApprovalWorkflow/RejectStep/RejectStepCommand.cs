using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalWorkflow.RejectStep
{
	public sealed record RejectStepCommand(
		Guid StepInstanceId,
		string Reason, 
		byte[] RowVersion
	) : IRequest<Unit>;
}
