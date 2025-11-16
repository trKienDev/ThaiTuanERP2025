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
		public async Task<IActionResult> GetFollowing([FromRoute] Guid PeriodId, CancellationToken cancellationToken) {
			var dtos = await _mediator.Send(new GetFollowingBudgetPlansQuery(PeriodId), cancellationToken);
			return Ok(ApiResponse<IReadOnlyList<BudgetPlanDto>>.Success(dtos));
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] BudgetPlanRequest request, CancellationToken cancellationToken) {
			var results = await _mediator.Send(new CreateBudgetPlanCommand(request), cancellationToken);
			return Ok(ApiResponse<Unit>.Success(results));	
		}
	}
}
