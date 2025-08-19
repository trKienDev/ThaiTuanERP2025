using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Finance.Commands.CashOutCodes.CreateCashOutCode;
using ThaiTuanERP2025.Application.Finance.Commands.CashOutCodes.DeleteCashOutCode;
using ThaiTuanERP2025.Application.Finance.Commands.CashOutCodes.ToggleCashOutCodeStatus;
using ThaiTuanERP2025.Application.Finance.Commands.CashOutCodes.UpdateCashOutCode;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Application.Finance.Queries.CashOutCodes.GetAllCashOutCodes;
using ThaiTuanERP2025.Application.Finance.Queries.CashOutCodes.GetCashOutCodeById;

namespace ThaiTuanERP2025.Api.Controllers.Finance
{
	
	public class CashOutCodeController : ControllerBase
	{
		private readonly IMediator _mediator;
		public CashOutCodeController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<ActionResult> GetAll() {
			var list = await _mediator.Send(new GetAllCashOutCodesQuery());
			return Ok(ApiResponse<List<CashOutCodeDto>>.Success(list));
		}

		[HttpGet("{id:guid}")]
		public async Task<ActionResult> GetById(Guid id) {
			var dto = await _mediator.Send(new GetCashOutCodeByIdQuery(id));
			return Ok(ApiResponse<CashOutCodeDto>.Success(dto)); 
		}

		[HttpPost]
		public async Task<ActionResult> Create([FromBody] CreateCashOutCodeCommand command) {
			var result = await _mediator.Send(command);
			return Ok(ApiResponse<CashOutCodeDto>.Success(result));
		}

		[HttpPut("{id:guid}")]
		public async Task<ActionResult> Update(Guid id, [FromBody] UpdateCashOutCodeCommand command) {
			var result = await _mediator.Send(command with { Id = id });
			return Ok(ApiResponse<CashOutCodeDto>.Success(result));
		}

		[HttpPatch("{id:guid}/status")]
		public async Task<ActionResult> ToggleStatus(Guid id, [FromBody] bool isActive) {
			var result = await _mediator.Send(new ToggleCashOutCodeStatusCommand(id, isActive));	
			return Ok(ApiResponse<bool>.Success(result));
		}

		[HttpDelete("{id:guid}")]
		public async Task<ActionResult> Delete(Guid id) {
			var result = await _mediator.Send(new DeleteCashOutCodeCommand(id));
			return Ok(ApiResponse<bool>.Success(result));
		}

	}
}
