using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Finance.Budgets.Commands.BudgetPeriods.CreateBudgetPeriod;
using ThaiTuanERP2025.Application.Finance.Budgets.Commands.BudgetPeriods.CreateBudgetPeriodsForYear;
using ThaiTuanERP2025.Application.Finance.Budgets.DTOs;
using ThaiTuanERP2025.Application.Finance.Budgets.Queries.BudgetPeriods.GetBudgetPeriodsForYear;
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

		[HttpPost("new")]
		public async Task<IActionResult> Create([FromBody] BudgetPeriodRequest request, CancellationToken cancellationToken) {
			var result = await _mediator.Send(new CreateBudgetPeriodCommand(request), cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));
		}

		[HttpPost("auto-generate")]
		public async Task<IActionResult> AutoGenerate( [FromBody] AutoGenerateYearRequest request, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new CreateBudgetPeriodsForYearCommand(request.Year), cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));
		}
	}
}
