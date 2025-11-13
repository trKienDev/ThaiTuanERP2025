using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Shared;
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

		[HttpGet]
		public async Task<IActionResult> GetAll(CancellationToken cancellationToken) {
			var dtos = await _mediator.Send(new GetAllBudgetApproversQuery(), cancellationToken);
			return Ok(ApiResponse<IReadOnlyList<BudgetApproverDto>>.Success(dtos));
		}

		[HttpGet("by-user-department")]
		public async Task<IActionResult> GetByUserDepartment(CancellationToken cancellationToken) {
			var dtos = await _mediator.Send(new GetBudgetApproversByUserDepartmentQuery(), cancellationToken);
			return Ok(ApiResponse<IReadOnlyList<BudgetApproverDto>>.Success(dtos));
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateBudgetApproverCommand command, CancellationToken cancellationToken) {
			var result = await _mediator.Send(command, cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));	
		}

		[HttpPut("{id:guid}")]
		public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateBudgetApproverRequest request, CancellationToken cancellationToken) {
			var result = await _mediator.Send(new UpdateBudgetApproverCommand(id, request), cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));
		}
	}
}
