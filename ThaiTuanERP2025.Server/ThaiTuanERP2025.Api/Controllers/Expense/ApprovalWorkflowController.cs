using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Expense.Commands.ApprovalWorkflows.CreateApprovalWorkflow;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Application.Expense.Requests;

namespace ThaiTuanERP2025.Api.Controllers.Expense
{
	[Authorize]
	[ApiController]
	[Route("/api/expense/approval-workflows")]
	public class ApprovalWorkflowController : Controller
	{
		private readonly IMediator _mediator;
		public ApprovalWorkflowController(IMediator mediator)
		{
			_mediator = mediator;
		}	

		[HttpPost]
		public async Task<ActionResult<ApiResponse<ApprovalWorkflowDto>>> Create([FromBody] CreateApprovalWorkflowRequest request, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new CreateApprovalWorkflowCommand(request) , cancellationToken);
			return ApiResponse<ApprovalWorkflowDto>.Success(result);
		}
	}
}
