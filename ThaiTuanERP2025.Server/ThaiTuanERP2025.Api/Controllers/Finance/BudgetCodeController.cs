using MediatR;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Finance.Commands.BudgetCodes.CreateBudgetCode;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Application.Finance.Queries.BudgetCodes.GetAllBudgetCodesQuery;

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

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateBudgetCodeCommand command)
		{
			var result = await _mediator.Send(command);
			return Ok(ApiResponse<BudgetCodeDto>.Success(result));
		}

		[HttpGet("all")]
		public async Task<IActionResult> GetAll()
		{
			var result = await _mediator.Send(new GetAllBudgetCodesQuery());
			return Ok(ApiResponse<List<BudgetCodeDto>>.Success(result));
		}
	}
}
