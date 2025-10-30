using MediatR;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Finance.Commands.BudgetCodes.CreateBudgetCode;
using ThaiTuanERP2025.Application.Finance.Commands.BudgetCodes.ToggleBudgetCodeActive;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Application.Finance.Queries.BudgetCodes.GetAllActiveBudgetCodes;
using ThaiTuanERP2025.Application.Finance.Queries.BudgetCodes.GetAllBudgetCodes;
using ThaiTuanERP2025.Application.Finance.Queries.BudgetCodes.GetBudgetCodesWithAmountForPeriod;

namespace ThaiTuanERP2025.Api.Controllers.Finance
{
	[Route("api/budget-code")]
	[ApiController]
	public class BudgetCodeController : ControllerBase
	{
		private readonly IMediator _mediator;
		public BudgetCodeController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("all")]
		public async Task<IActionResult> GetAll()
		{
			var result = await _mediator.Send(new GetAllBudgetCodesQuery());
			return Ok(ApiResponse<List<BudgetCodeDto>>.Success(result));
		}

		[HttpGet("active")]
		public async Task<IActionResult> GetAllActive() {
			var codes = await _mediator.Send(new GetAllActiveBudgetCodesQuery());
			return Ok(ApiResponse<List<BudgetCodeDto>>.Success(codes));
		}

		[HttpGet("with-current-amount")]
		public async Task<IActionResult> GetWithAmount([FromQuery] int? year, [FromQuery] int? month,  CancellationToken cancellationToken)
		{
			// var data = await _mediator.Send(new GetBudgetCodesWithAmountForPeriodQuery { Year = year, Month = month, DepartmentId = departmentId }, cancellationToken);
			var data = await _mediator.Send(new GetBudgetCodesWithAmountForPeriodQuery ( year,  month), cancellationToken);
			return Ok(ApiResponse<List<BudgetCodeWithAmountDto>>.Success(data));
		}

		[HttpPost("new")]
		public async Task<IActionResult> Create([FromBody] CreateBudgetCodeCommand command)
		{
			var result = await _mediator.Send(command);
			return Ok(ApiResponse<BudgetCodeDto>.Success(result));
		}

		[HttpPut("{id}/toggle-active")]
		public async Task<IActionResult> ToggleActvie(Guid id, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new ToggleBudgetCodeActiveCommand(id), cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));
		}
	}
}
