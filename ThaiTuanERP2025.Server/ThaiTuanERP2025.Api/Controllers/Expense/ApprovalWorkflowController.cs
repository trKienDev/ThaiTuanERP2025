using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Expense.Commands.ApprovalWorkflow.ApproveStep;
using ThaiTuanERP2025.Application.Expense.Commands.ApprovalWorkflow.CommentOnStep;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Application.Expense.Queries.ApprovalWorkflows.GetFlowInstanceByDocument;
using ThaiTuanERP2025.Application.Expense.Queries.ApprovalWorkflows.GetMyPendingApproval;

namespace ThaiTuanERP2025.Api.Controllers.Expense
{
	[ApiController]
	[Authorize]
	[Route("api/expense-approval-workflow")]
	[Produces("application/json")]
	public class ApprovalWorkflowController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly IAuthorizationService _auth;

		private static class Policies
		{
			public const string CanApproveStep = "CanApproveStep";
			public const string CanCommentStep = "CanCommentStep";
		}

		public ApprovalWorkflowController(IMediator mediator, IAuthorizationService auth)
		{
			_mediator = mediator;
			_auth = auth;
		}

		/// <summary>Get flow instance for a specific document.</summary>
		[HttpGet("approvals/docs/{docType}/{docId:guid}")]
		[ProducesResponseType(typeof(ApiResponse<FlowInstanceDto>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ApiResponse<FlowInstanceDto>>> GetFlowForDocument(
		    string docType, Guid docId, CancellationToken cancellationToken)
		{
			if (string.IsNullOrWhiteSpace(docType))
				return BadRequest(ApiResponse<FlowInstanceDto>.Fail("docType is required."));

			var result = await _mediator.Send(new GetFlowInstanceByDocumentQuery(docType, docId), cancellationToken);
			return Ok(ApiResponse<FlowInstanceDto>.Success(result));
		}

		/// <summary>Get my pending approval steps.</summary>
		[HttpGet("approvals/my-pending")]
		[ProducesResponseType(typeof(ApiResponse<IReadOnlyList<MyPendingApprovalDto>>), StatusCodes.Status200OK)]
		public async Task<ActionResult<ApiResponse<IReadOnlyList<MyPendingApprovalDto>>>> GetMyPending(
		    CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new GetMyPendingApprovalQuery(), cancellationToken);
			return Ok(ApiResponse<IReadOnlyList<MyPendingApprovalDto>>.Success(result));
		}

		/// <summary>Approve a step (requires CanApproveStep).</summary>
		[HttpPost("approvals/steps/{stepId:guid}/approve")]
		[ProducesResponseType(typeof(ApiResponse<Unit>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public async Task<ActionResult<ApiResponse<Unit>>> Approve(
		    Guid stepId, [FromBody] ApproveStepDto dto, CancellationToken cancellationToken)
		{
			var authz = await _auth.AuthorizeAsync(User, stepId, Policies.CanApproveStep);
			if (!authz.Succeeded)
				return Forbid();

			var result = await _mediator.Send(new ApproveStepCommand(stepId, dto.Comment, dto.RowVersion), cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));
		}

		/// <summary>Comment on a step (requires CanCommentStep).</summary>
		[HttpPost("approvals/steps/{stepId:guid}/comments")]
		[ProducesResponseType(typeof(ApiResponse<Unit>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public async Task<ActionResult<ApiResponse<Unit>>> Comment(
		    Guid stepId, [FromBody] CommentOnStepDto dto, CancellationToken cancellationToken)
		{
			var authz = await _auth.AuthorizeAsync(User, stepId, Policies.CanCommentStep);
			if (!authz.Succeeded)
				return Forbid();

			var result = await _mediator.Send(new CommentOnStepCommand(stepId, dto.Comment, dto.AttachmentFileIds), cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));
		}
	}
}
