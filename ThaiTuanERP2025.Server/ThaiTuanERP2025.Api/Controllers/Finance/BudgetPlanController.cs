using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using ThaiTuanERP2025.Api.Shared;
using ThaiTuanERP2025.Application.Finance.BudgetPlans.Commands;
using ThaiTuanERP2025.Application.Finance.BudgetPlans.Contracts;
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
			var dtos = await _mediator.Send(new GetFollowingBudgetPlansQuery(periodId), cancellationToken);
			return Ok(ApiResponse<IReadOnlyList<BudgetPlanDto>>.Success(dtos));
		}

		[HttpGet("{id:guid}")]
		public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken) {
			var dto = await _mediator.Send(new GetBudgetPlanByIdQuery(id), cancellationToken);
			return Ok(ApiResponse<BudgetPlanDto>.Success(dto));
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] BudgetPlanRequest request, CancellationToken cancellationToken) {
			var results = await _mediator.Send(new CreateBudgetPlanCommand(request), cancellationToken);
			return Ok(ApiResponse<Unit>.Success(results));	
		}

		[HttpPut("detail/{detailId:guid}/amount")]
		public async Task<IActionResult> UpdateDetailAmount([FromRoute] Guid detailId, [FromBody] decimal amount, CancellationToken cancellationToken) {
			var result = await _mediator.Send(new UpdateBudgetPlanDetailAmountCommand(detailId, amount), cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));
		}

		[HttpPost("{id}/review")]
		public async Task<IActionResult> MarkReview([FromRoute] Guid Id, CancellationToken cancellationToken) {
			var result = await _mediator.Send(new ReviewBudgetPlanCommand(Id), cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));
		}
	}
}
