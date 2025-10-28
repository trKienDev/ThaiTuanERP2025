using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Finance.Commands.CashoutCodes.DeleteCashoutCode;
using ThaiTuanERP2025.Application.Finance.Commands.CashoutCodes.ToggleCashoutCodeActivate;
using ThaiTuanERP2025.Application.Finance.Commands.CashoutCodes.UpdateCashoutCode;
using ThaiTuanERP2025.Application.Finance.Commands.CashoutCodes.CreateCashoutCode;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Application.Finance.Queries.CashoutCodes.GetAllCashoutCodes;
using ThaiTuanERP2025.Application.Finance.Queries.CashoutCodes.GetCashoutCodeById;

namespace ThaiTuanERP2025.Api.Controllers.Finance
{
	[ApiController]
	[Authorize]
	[Route("api/cashout-codes")]
	public class CashoutCodeController : ControllerBase
	{
		private readonly IMediator _mediator;
		public CashoutCodeController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("all")]
		public async Task<ActionResult> GetAll() {
			var list = await _mediator.Send(new GetAllCashoutCodesQuery());
			return Ok(ApiResponse<List<CashoutCodeDto>>.Success(list));
		}

		[HttpGet("{id:guid}")]
		public async Task<ActionResult> GetById(Guid id) {
			var dto = await _mediator.Send(new GetCashoutCodeByIdQuery(id));
			return Ok(ApiResponse<CashoutCodeDto>.Success(dto)); 
		}

		[HttpPost("new")]
		public async Task<ActionResult> Create([FromBody] CreateCashoutCodeCommand command) {
			var result = await _mediator.Send(command);
			return Ok(ApiResponse<CashoutCodeDto>.Success(result));
		}

		[HttpPut("{id:guid}")]
		public async Task<ActionResult> Update(Guid id, [FromBody] UpdateCashoutCodeCommand command) {
			var result = await _mediator.Send(command with { Id = id });
			return Ok(ApiResponse<CashoutCodeDto>.Success(result));
		}

		[HttpPatch("{id:guid}/status")]
		public async Task<ActionResult> ToggleStatus(Guid id, [FromBody] bool isActive) {
			var result = await _mediator.Send(new ToggleCashoutCodeActivateCommand(id, isActive));	
			return Ok(ApiResponse<bool>.Success(result));
		}

		[HttpDelete("{id:guid}")]
		public async Task<ActionResult> Delete(Guid id) {
			var result = await _mediator.Send(new DeleteCashoutCodeCommand(id));
			return Ok(ApiResponse<bool>.Success(result));
		}

	}
}
