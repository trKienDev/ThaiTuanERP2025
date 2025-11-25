using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Shared;
using ThaiTuanERP2025.Api.Security;
using ThaiTuanERP2025.Application.Finance.BudgetPeriods;
using ThaiTuanERP2025.Application.Finance.BudgetPeriods.Commands;
using ThaiTuanERP2025.Application.Finance.BudgetPeriods.Queries;

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

		[HttpGet]
		public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new GetAllBudgetPeriodsQuery(), cancellationToken);
			return Ok(ApiResponse<IReadOnlyList<BudgetPeriodLookupDto>>.Success(result));
		}

		[HttpGet("year/{year:int}")]
		public async Task<IActionResult> GetForYear([FromRoute] int year, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new GetBudgetPeriodsForYearQuery(year), cancellationToken);
			return Ok(ApiResponse<IReadOnlyList<BudgetPeriodDto>>.Success(result));
		}

		[HttpGet("years")]
		public async Task<IActionResult> GetYears(CancellationToken cancellationToken) {
			var result = await _mediator.Send(new GetYearsOfBudgetPeriodQuery(), cancellationToken);
			return Ok(ApiResponse<IReadOnlyList<int>>.Success(result));	
		}

		[HttpGet("available")]
		public async Task<IActionResult> GetAvailable(CancellationToken cancellationToken) {
			var result = await _mediator.Send(new GetAvailableBudgetPeriodQuery(), cancellationToken);
			return Ok(ApiResponse<IReadOnlyList<BudgetPeriodDto>>.Success(result));
		}

		[HasPermission("budget-period.create-for-year")]
		[HttpPost("year/{year:int}")]
		public async Task<IActionResult> CreateForYear([FromRoute] int year, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new CreateBudgetPeriodsForYearCommand(year), cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));
		}

		[HttpPut("{id:guid}")]
		public async Task<IActionResult> UpdateDate([FromRoute] Guid Id, [FromBody] UpdateBudgetPeriodRequest request, CancellationToken cancellationToken) {
			var result = await _mediator.Send(new UpdateBudgetPeriodCommand(Id, request), cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));
		}
	}
}
