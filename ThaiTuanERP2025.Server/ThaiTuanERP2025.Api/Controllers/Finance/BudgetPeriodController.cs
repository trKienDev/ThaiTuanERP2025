using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Finance.BudgetPeriods.Commands.CreateForYear;
using ThaiTuanERP2025.Application.Finance.BudgetPeriods.Queries.GetForYear;
using ThaiTuanERP2025.Application.Finance.Budgets.DTOs;
using ThaiTuanERP2025.Application.Finance.Budgets.Requests;

namespace ThaiTuanERP2025.Api.Controllers.Finance
{
	[Authorize]
	[Route("api/budget-period")]
	[ApiController]
	public class BudgetPeriodController : ControllerBase
	{
		private readonly IMediator _mediator;
		public BudgetPeriodController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("for-year/{year:int}")]
		public async Task<IActionResult> GetForYear([FromRoute] int year, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new GetBudgetPeriodsForYearQuery(year), cancellationToken);
			return Ok(ApiResponse<IReadOnlyList<BudgetPeriodDto>>.Success(result));
		}

		[HttpPost("for-year")]
		public async Task<IActionResult> CreateForYear([FromBody] int year, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new CreateBudgetPeriodsForYearCommand(year), cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));
		}
	}
}
