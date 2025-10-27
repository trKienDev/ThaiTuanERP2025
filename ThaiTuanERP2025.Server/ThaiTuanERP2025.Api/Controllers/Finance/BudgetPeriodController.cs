using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Finance.Commands.BudgetPeridos.CreateBudgetPeriod;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Application.Finance.Queries.BudgetPeriods.GetAllActiveBudgetPeriods;
using ThaiTuanERP2025.Application.Finance.Queries.BudgetPeriods.GetAllBudgetPeriods;

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

		[HttpGet("all")]
		public async Task<IActionResult> GetAll()
		{
			var budgetPeriods = await _mediator.Send(new GetAllBudgetPeriodsQuery());
			return Ok(ApiResponse<List<BudgetPeriodDto>>.Success(budgetPeriods));
		}

		[HttpGet("active")]
		public async Task<IActionResult> GetAllActive() {
			var result = await _mediator.Send(new GetAllActiveBudgetPeriodsQuery());
			return Ok(ApiResponse<List<BudgetPeriodDto>>.Success(result));
		}

		[HttpPost("new")]
		public async Task<IActionResult> Create(CreateBudgetPeriodCommand command) {
			var result = await _mediator.Send(command);
			return Ok(ApiResponse<BudgetPeriodDto>.Success(result));
		}
	}
}
