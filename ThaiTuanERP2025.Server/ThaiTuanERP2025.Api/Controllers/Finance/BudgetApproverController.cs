using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Finance.BudgetApprovers;
using ThaiTuanERP2025.Application.Finance.BudgetApprovers.Commands;
using ThaiTuanERP2025.Application.Finance.BudgetApprovers.Queries;

namespace ThaiTuanERP2025.Api.Controllers.Finance
{
	[Authorize]
	[ApiController]
	[Route("api/budget-approver")]
	public class BudgetApproverController : ControllerBase
	{
		private readonly IMediator _mediator;
		public BudgetApproverController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("all")]
		public async Task<IActionResult> GetAll(CancellationToken cancellationToken) {
			var dtos = await _mediator.Send(new GetAllBudgetApproversQuery(), cancellationToken);
			return Ok(ApiResponse<IReadOnlyList<BudgetApproverDto>>.Success(dtos));
		}

		[HttpGet("by-user-department")]
		public async Task<IActionResult> GetByUserDepartment(CancellationToken cancellationToken) {
			var dtos = await _mediator.Send(new GetBudgetApproversByUserDepartmentQuery(), cancellationToken);
			return Ok(ApiResponse<IReadOnlyList<BudgetApproverDto>>.Success(dtos));
		}

		[HttpPost("new")]
		public async Task<IActionResult> Create([FromBody] CreateBudgetApproverCommand command, CancellationToken cancellationToken) {
			var result = await _mediator.Send(command, cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));	
		}

		[HttpPut("{id:guid}/departments")]
		public async Task<IActionResult> UpdateDepartments(Guid id, [FromBody] UpdateBudgetApproverDepartmentRequest request, CancellationToken cancellationToken) {
			var result = await _mediator.Send(new UpdateBudgetApproverDepartmentCommand(id, request), cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));
		}
	}
}
