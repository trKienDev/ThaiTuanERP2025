using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Shared;
using ThaiTuanERP2025.Application.Finance.CashoutCodes;
using ThaiTuanERP2025.Application.Finance.CashoutCodes.Commands;
using ThaiTuanERP2025.Application.Finance.CashoutCodes.Contracts;
using ThaiTuanERP2025.Application.Finance.CashoutCodes.Queries;
using ThaiTuanERP2025.Application.Finance.CashoutGroups.Contracts;

namespace ThaiTuanERP2025.Api.Controllers.Finance
{
	[Authorize]
	[ApiController]
	[Route("api/cashout-code")]
	public class CashoutCodeController : ControllerBase
	{
		private readonly IMediator _mediator;
		public CashoutCodeController(IMediator mediator) => _mediator = mediator;

		[HttpGet("tree")]		
		public async Task<IActionResult> GetTree(CancellationToken cancellationToken)
		{
			var tree = await _mediator.Send(new GetCashoutCodeWithGroupTreeQuery(), cancellationToken);
			return Ok(ApiResponse<IReadOnlyList<CashoutGroupTreeWithCodesDto>>.Success(tree));
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CashoutCodePayload payload, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new CreateCashoutCodeCommand(payload), cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));
		}
	}
}
