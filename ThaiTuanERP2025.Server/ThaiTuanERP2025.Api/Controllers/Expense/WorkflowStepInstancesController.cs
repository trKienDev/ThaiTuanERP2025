using MediatR;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Expense.Commands.ApprovalSteps.OverrideApprover;
using ThaiTuanERP2025.Application.Expense.Commands.ApprovalSteps.RejectStep;
using ThaiTuanERP2025.Application.Expense.Commands.ApprovalSteps.ReopenStep;
using ThaiTuanERP2025.Application.Expense.Commands.ApprovalSteps.SkipStep;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Application.Expense.Request;

namespace ThaiTuanERP2025.Api.Controllers.Expense
{
	[ApiController]
	[Route("api/workflows/{workflowId:guid}/steps")]
	public class WorkflowStepInstancesController : ControllerBase
	{
		private readonly IMediator _mediator;
		public WorkflowStepInstancesController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost("{stepId:guid}/reject")]
		public async Task<IActionResult> Reject(Guid workflowId, Guid stepId, [FromBody] RejectStepRequest body, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new RejectStepCommand(workflowId, stepId, body ?? new()), cancellationToken);
			return Ok(ApiResponse<ApprovalStepInstanceDto>.Success(result, "Rejected"));
		}

		[HttpPost("{stepId:guid}/skip")]
		public async Task<IActionResult> Skip(Guid workflowId, Guid stepId, [FromBody] string reason, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new SkipStepCommand(workflowId, stepId, reason), cancellationToken);
			return Ok(ApiResponse<ApprovalStepInstanceDto>.Success(result, "Skipped"));
		}

		[HttpPost("{stepId:guid}/override-approver")]
		public async Task<IActionResult> OverrideApprover(Guid workflowId, Guid stepId, [FromBody] OverrideApproverRequest body, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new OverrideApproverCommand(workflowId, stepId, body), cancellationToken);
			return Ok(ApiResponse<ApprovalStepInstanceDto>.Success(result, "Overridden"));
		}

		[HttpPost("{stepId:guid}/reopen")]
		public async Task<IActionResult> Reopen(Guid workflowId, Guid stepId, [FromBody] ReopenStepRequest body, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new ReopenStepCommand(workflowId, stepId, body), cancellationToken);
			return Ok(ApiResponse<ApprovalStepInstanceDto>.Success(result, "Reopened"));
		}
	}
}
