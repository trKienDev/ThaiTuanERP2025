using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Shared;
using ThaiTuanERP2025.Application.Finance.CashoutCodes;
using ThaiTuanERP2025.Application.Finance.CashoutCodes.Queries;

namespace ThaiTuanERP2025.Api.Controllers.Finance
{
	[Authorize]
	[ApiController]
	[Route("api/cashout-code")]
	public class CashoutCodeController : ControllerBase
	{
		private readonly IMediator _mediator;
		public CashoutCodeController(IMediator mediator) => _mediator = mediator;

		[HttpGet]
		public async Task<IActionResult> GetAll(CancellationToken cancellationToken) {
			var dtos = await _mediator.Send(new GetAllCashoutCodesQuery(), cancellationToken);
			return Ok(ApiResponse<IReadOnlyList<CashoutCodeDto>>.Success(dtos));	
		}
	}
}
