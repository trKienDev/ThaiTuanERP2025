using MediatR;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Presentation.Common;
using ThaiTuanERP2025.Application.Expense.Commands.ApprovalWorkflows.CreateApprovalWorkflowTemplate;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Application.Expense.Queries.ApprovalWorkflow.GetApprovalWorkflowTemplateDetail;
using ThaiTuanERP2025.Application.Expense.Queries.ApprovalWorkflow.GetWorkflowTemplateById;
using ThaiTuanERP2025.Application.Expense.Queries.ApprovalWorkflow.GetWorkflowTemplatesByFilter;

namespace ThaiTuanERP2025.Presentation.Controllers.Expense
{
	[ApiController]
	[Route("api/workflow-templates")]
	public class WorkflowTemplateController : ControllerBase
	{
		private readonly IMediator _mediator;
		public WorkflowTemplateController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("{id:guid}")]
		public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken) {
			var result = await _mediator.Send(new GetWorkflowTemplateByIdQuery(id), cancellationToken);
			if(result is null)
				return NotFound(ApiResponse<string>.Fail("Không tìm thấy luồng phê duyệt"));
			return Ok(ApiResponse<ApprovalWorkflowTemplateDto>.Success(result));
		}

		[HttpGet("all")] 
		public async Task<IActionResult> GetList([FromQuery] string? documentType, [FromQuery] bool? isActive, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new GetWorkflowTemplatesByFilterQuery(documentType, isActive), cancellationToken);
			return Ok(ApiResponse<IReadOnlyList<ApprovalWorkflowTemplateDto>>.Success(result));
		}

		[HttpGet("{id:guid}/detail")]
		public async Task<IActionResult> GetDetail(Guid id, CancellationToken cancellationToken) {
			var result = await _mediator.Send(new GetApprovalWorkflowTemplateDetailQuery(id), cancellationToken);
			if (result is null)
				return BadRequest(ApiResponse<string>.Fail("Không tìm thấy workflow template detail"));
			return Ok(ApiResponse<ApprovalWorkflowTemplateDetailDto>.Success(result));
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] ApprovalWorkflowTemplateRequest body, CancellationToken cancellationToken) {
			if(body is null) 
				return BadRequest(ApiResponse<string>.Fail("Dữ liệu không hợp lệ"));

			var result = await _mediator.Send(new CreateApprovalWorkflowTemplateCommand(body), cancellationToken);
			return Ok(ApiResponse<ApprovalWorkflowTemplateDto>.Success(result, "Tạo luồng phê duyệt thành công"));
		}

		
	}
}
