using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Application.Expense.Queries.ApprovalWorkflows.GetFlowInstanceByDocument;
using ThaiTuanERP2025.Application.Expense.Queries.ApprovalWorkflows.GetMyPendingApproval;
using ThaiTuanERP2025.Application.Expense.Queries.ApprovalWorkflows.GetStepAction;

namespace ThaiTuanERP2025.Api.Controllers.Expense
{
	[ApiController]
	[Authorize]
	[Route("api/expense-approval-workflow")]
	public class ApprovalWorkflowController : ControllerBase
	{
		private IMediator _mediator;
		public ApprovalWorkflowController(IMediator mediator) {
			_mediator = mediator;	
		}


		[HttpGet("approvals/docs/{docType}/{docId:guid}")]
		public async Task<ActionResult<FlowInstanceDto>> GetFlowForDocument(string docType, Guid docId, CancellationToken cancellationToken) {
			var result = await _mediator.Send(new GetFlowInstanceByDocumentQuery(docType, docId), cancellationToken);
			return Ok(ApiResponse<FlowInstanceDto>.Success(result));
		}

		[HttpGet("approvals/my-pending")]
		public async Task<ActionResult<MyPendingApprovalDto>> GetMyPending(CancellationToken cancellationToken) {
			var result = await _mediator.Send(new GetMyPendingApprovalQuery(), cancellationToken);
			return Ok(ApiResponse<IReadOnlyList<MyPendingApprovalDto>>.Success(result));
		}
	}
}
