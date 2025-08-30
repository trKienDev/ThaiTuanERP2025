using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Expense.Commands.ApprovalWorkflow.CommentOnStep
{
	public sealed record CommentOnStepCommand(
		Guid StepInstanceId,
		string? Comment, // cho phép trống nếu upload file
		List<Guid>? AttachmentFileIds // optional
	) : IRequest<Unit>;
}
