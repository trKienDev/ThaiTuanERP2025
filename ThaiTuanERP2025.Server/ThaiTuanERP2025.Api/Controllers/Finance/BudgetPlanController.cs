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

		[HttpGet("following")]
		public async Task<IActionResult> GetByMyDepartment(CancellationToken cancellationToken) {
			var result = await _mediator.Send(new GetFollowingBudgetPlansQuery(), cancellationToken);
			return Ok(ApiResponse<IReadOnlyList<BudgetPlanDto>>.Success(result));
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateBudgetPlanCommand command, CancellationToken cancellationToken) {
			var result = await _mediator.Send(command, cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));
		}
	}
}
