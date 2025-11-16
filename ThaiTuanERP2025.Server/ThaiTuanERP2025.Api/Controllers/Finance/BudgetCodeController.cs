using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Shared;
using ThaiTuanERP2025.Application.Finance.BudgetCodes;
using ThaiTuanERP2025.Application.Finance.BudgetCodes.Commands;
using ThaiTuanERP2025.Application.Finance.BudgetCodes.Queries;

namespace ThaiTuanERP2025.Api.Controllers.Finance
{
	[Authorize]
	[ApiController]
	[Route("api/budget-code")]
	public class BudgetCodeController : ControllerBase
	{
		private readonly IMediator _mediator;
		public BudgetCodeController(IMediator mediator) => _mediator = mediator;

		[HttpGet]
		public async Task<IActionResult> GetAll(CancellationToken cancellationToken) {
			var dtos = await _mediator.Send(new GetAllBudgetCodesQuery(), cancellationToken);
			return Ok(ApiResponse<IReadOnlyList<BudgetCodeDto>>.Success(dtos));	
		}

		//[HttpGet("available")]
		//public async Task<IActionResult> GetAvailable(CancellationToken cancellationToken)
		//{
		//	var dtos = await _mediator.Send(new GetAvailabelBudgetCodesQuery(), cancellationToken);
		//	return Ok(ApiResponse<IReadOnlyList<BudgetCodeLookupDto>>.Success(dtos));
		//}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateBudgetCodeCommand command, CancellationToken cancellationToken) {
			var result = await _mediator.Send(command, cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));
		}
	}
}
