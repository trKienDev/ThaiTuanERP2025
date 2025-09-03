using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Expense.Commands.ApprovalWorkflows.CreateApprovalWorkflow;
using ThaiTuanERP2025.Application.Expense.Commands.ApprovalWorkflows.UpdateApprovalWorkflow;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Application.Expense.Queries.ApprovalWorkflows.GetAllApprovalWorkflow;
using ThaiTuanERP2025.Application.Expense.Queries.ApprovalWorkflows.GetApprovalWorkflowById;
using ThaiTuanERP2025.Application.Expense.Requests;

namespace ThaiTuanERP2025.Api.Controllers.Expense
{
	[Authorize]
	[ApiController]
	[Route("api/expense/approval-workflows")]
	public class ApprovalWorkflowController : Controller
	{
		private readonly IMediator _mediator;
		public ApprovalWorkflowController(IMediator mediator)
		{
			_mediator = mediator;
		}	

		[HttpGet("all")]
		public async Task<ActionResult<ApiResponse<IReadOnlyList<ApprovalWorkflowDto>>>> GetAll(CancellationToken cancellationToken) {
			var dtos = await _mediator.Send(new GetAllApprovalWorkflowQuery(), cancellationToken);
			return Ok(ApiResponse<IReadOnlyList<ApprovalWorkflowDto>>.Success(dtos));
		}

		[HttpGet("{id:guid}")]	
		public async Task<ActionResult<ApiResponse<ApprovalWorkflowDto>>> GetById(Guid Id, CancellationToken cancellationToken) {
			var dto = await _mediator.Send(new GetApprovalWorkflowByIdQuery(Id), cancellationToken);
			if (dto is null) 
				return NotFound(ApiResponse<ApprovalWorkflowDto>.Fail("Not found"));

			return Ok(ApiResponse<ApprovalWorkflowDto>.Success(dto));

		}

		[HttpPost]
		public async Task<ActionResult<ApiResponse<ApprovalWorkflowDto>>> Create([FromBody] CreateApprovalWorkflowRequest request, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new CreateApprovalWorkflowCommand(request) , cancellationToken);
			return Ok(ApiResponse<ApprovalWorkflowDto>.Success(result));
		}

		[HttpPut("{id:guid}")]	
		public async Task<ActionResult<ApiResponse<ApprovalWorkflowDto>>> Update(Guid id, [FromBody] UpdateApprovalWorkflowRequest request, CancellationToken cancellationToken)
		{
			var dto = await _mediator.Send(new UpdateApprovalWorkflowCommand(id, request), cancellationToken);
			if (dto is null)
				return NotFound(ApiResponse<ApprovalWorkflowDto>.Fail("Not found"));
			return Ok(ApiResponse<ApprovalWorkflowDto>.Success(dto));
		}
	}
}
