using MediatR;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Expense.Commands.ApprovalSteps.CreateApprovalStepTemplate;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Application.Expense.Queries.ApprovalSteps.GetStepTemplatesByWorkflowId;

namespace ThaiTuanERP2025.Api.Controllers.Expense
{
	[ApiController]
	[Route("api/workflow-templates/{workflowTemplateId:guid}/steps")]
	public class WorkflowStepTemplatesController : ControllerBase
	{
		private readonly IMediator _mediator;
		public WorkflowStepTemplatesController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<IActionResult> GetList(Guid workflowTemplateId, CancellationToken cancellationToken) {
			var result = await _mediator.Send(new GetStepTemplatesByWorkflowIdQuery(workflowTemplateId), cancellationToken);
			return Ok(ApiResponse<IReadOnlyList<ApprovalStepTemplateDto>>.Success(result));
		}

		[HttpPost]
		public async Task<IActionResult> Create(Guid workflowTemplateId, [FromBody] ApprovalStepTemplateRequest body, CancellationToken cancellationToken) {
			if (body is null || body.WorkflowTemplateId == Guid.Empty || body.WorkflowTemplateId != workflowTemplateId)
				return BadRequest(ApiResponse<string>.Fail("Dữ liệu không hợp lệ"));
			
			var result = await _mediator.Send(new CreateApprovalStepTemplateCommand(body), cancellationToken);
			return Ok(ApiResponse<ApprovalStepTemplateDto>.Success(result, "Tạo bước phê duyệt thành công"));
		}							
	}
}
