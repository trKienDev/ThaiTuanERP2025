using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Shared;
using ThaiTuanERP2025.Application.Finance.BudgetPlans;
using ThaiTuanERP2025.Application.Finance.BudgetPlans.Commands;
using ThaiTuanERP2025.Application.Finance.BudgetPlans.Queries;

namespace ThaiTuanERP2025.Api.Controllers.Finance
{
	[Authorize]
	[ApiController]
	[Route("api/budget-plan")]
	public class BudgetPlanController : ControllerBase
	{
		private readonly IMediator _mediator;
		public BudgetPlanController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("following/{periodId:guid}")]
		public async Task<IActionResult> GetFollowing([FromRoute] Guid periodId, CancellationToken cancellationToken) {
			var result = await _mediator.Send(new GetFollowingBudgetPlansByPeriodQuery(periodId), cancellationToken);
			return Ok(ApiResponse<IReadOnlyList<BudgetPlansByDepartmentDto>>.Success(result));
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateBudgetPlanCommand command, CancellationToken cancellationToken) {
			var result = await _mediator.Send(command, cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));
		}

		[HttpPost("{id:guid}/review")]
		public async Task<IActionResult> Review([FromRoute] Guid id, CancellationToken cancellationToken) {
			var result = await _mediator.Send(new ReviewBudgetPlanCommand(id), cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));
		}

		[HttpPut("{id:guid}/amount")]
		public async Task<IActionResult> UpdateAmount([FromRoute] Guid Id, [FromBody] decimal Amount, CancellationToken cancellationToken) {
			var result = await _mediator.Send(new UpdateBudgetPlanAmountCommand(Id, Amount), cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));
		}

	}
}
