using MediatR;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Expense.Commands.ApprovalWorkflows.CreateApprovalWorkflowInstance;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Application.Expense.Queries.ApprovalWorkflow.GetApprovalWorkflowInstanceDetail;
using ThaiTuanERP2025.Application.Expense.Queries.ApprovalWorkflow.GetApprovalWorkflowInstancesByFilter;
using ThaiTuanERP2025.Domain.Expense.Enums;

namespace ThaiTuanERP2025.Api.Controllers.Expense
{
	[ApiController]
	[Route("api/workflows")]
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
	}
}
