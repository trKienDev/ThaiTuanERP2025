using MediatR;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Presentation.Common;
using ThaiTuanERP2025.Application.Expense.Commands.ApprovalSteps.ApproveCurrentStep;
using ThaiTuanERP2025.Application.Expense.Commands.ApprovalSteps.RejectCurrentStep;
using ThaiTuanERP2025.Application.Expense.Commands.ApprovalWorkflows.CreateApprovalWorkflowInstance;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Application.Expense.Queries.ApprovalWorkflow.GetApprovalWorkflowInstanceDetail;
using ThaiTuanERP2025.Application.Expense.Queries.ApprovalWorkflow.GetApprovalWorkflowInstancesByFilter;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Presentation.Controllers.Expense
{
	[ApiController]
	[Route("api/approval-workflow-instances")]
	public class WorkflowInstanceController : ControllerBase
	{
		private readonly IMediator _mediator;
		public WorkflowInstanceController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<IActionResult> GetList(
			[FromQuery] string? documentType,
			[FromQuery] Guid? documentId,
			[FromQuery] WorkflowStatus? status,
			[FromQuery] string? budgetCode,
			[FromQuery] decimal? minAmount,
			[FromQuery] decimal? maxAmount,
			CancellationToken cancellationToken
		) {
			var result = await _mediator.Send(new GetApprovalWorkflowInstancesByFilterQuery(
				documentType, documentId, status, budgetCode, minAmount, maxAmount
			), cancellationToken);

			return Ok(ApiResponse<IReadOnlyList<ApprovalWorkflowInstanceDto>>.Success(result));
		}

		[HttpGet("{id:guid}")]
		public async Task<IActionResult> GetDetail(Guid id, CancellationToken cancellationToken) {
			var result = await _mediator.Send(new GetApprovalWorkflowInstanceDetailQuery(id), cancellationToken);
			return Ok(ApiResponse<ApprovalWorkflowInstanceDetailDto>.Success(result));
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] ApprovalWorkflowInstanceRequest body, CancellationToken cancellationToken) {
			if (body is null)
				return BadRequest(ApiResponse<string>.Fail("Dữ liệu không hợp lệ"));
			var result = await _mediator.Send(new CreateApprovalWorkflowInstanceCommand(body), cancellationToken);
			return Ok(ApiResponse<ApprovalWorkflowInstanceDto>.Success(result, "Tạo snapshot luồng duyệt thành công"));
		}

		[HttpPost("{instanceId}/steps/{stepId}/approve")]
		public async Task<IActionResult> ApproveStep(Guid instanceId, Guid stepId, [FromBody] ApproveStepRequest body, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new ApproveCurrentStepCommand(instanceId, stepId, body.UserId, body.PaymentId, body.Comment), cancellationToken);
			return Ok(ApiResponse<object>.Success("Step approved successfully"));
		}
		[HttpPost("{instanceId}/steps/{stepId}/reject")]
		public async Task<IActionResult> RejectStep(Guid instanceId, Guid stepId, [FromBody] RejectStepRequest body, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new RejectCurrentStepCommand(instanceId, stepId, body.UserId, body.PaymentId, body.Comment), cancellationToken);
			return Ok(ApiResponse<object>.Success("Step rejected successfully"));
		}
	}
}
