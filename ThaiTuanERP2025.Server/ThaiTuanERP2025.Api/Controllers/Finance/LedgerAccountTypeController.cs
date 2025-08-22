using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Finance.Commands.LedgerAccountTypes.CreateLedgerAccountType;
using ThaiTuanERP2025.Application.Finance.Commands.LedgerAccountTypes.DeleteLedgerAccountType;
using ThaiTuanERP2025.Application.Finance.Commands.LedgerAccountTypes.ToggleLedgerAccountTypeStatus;
using ThaiTuanERP2025.Application.Finance.Commands.LedgerAccountTypes.UpdateAccountType;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Application.Finance.Queries.LedgerAccountTypes.GetAllLedgerAccountTypes;
using ThaiTuanERP2025.Application.Finance.Queries.LedgerAccountTypes.GetLedgerAccountTypeById;

namespace ThaiTuanERP2025.Api.Controllers.Finance
{
	[Authorize]
	[ApiController]
	[Route("api/ledger-account-types")]
	public class LedgerAccountTypeController : ControllerBase
	{
		private readonly IMediator _mediator;
		public LedgerAccountTypeController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<ActionResult> GetAll() {
			var list = await _mediator.Send(new GetAllLedgerAccountTypesQuery());
			return Ok(ApiResponse<List<LedgerAccountTypeDto>>.Success(list));
		}

		[HttpGet("{id:guid}")]
		public async Task<ActionResult> GetById(Guid id) {
			var dto = await _mediator.Send(new GetLedgerAccountTypeByIdQuery(id));
			return Ok(ApiResponse<LedgerAccountTypeDto>.Success(dto));
		}

		[HttpPost]
		public async Task<ActionResult> Create([FromBody] CreateLedgerAccountTypeCommand command) {
			var result = await _mediator.Send(command);
			return Ok(ApiResponse<LedgerAccountTypeDto>.Success(result));	
		}

		[HttpPut("{id:guid}")]
		public async Task<ActionResult> Update(Guid id, [FromBody] UpdateLedgerAccountTypeCommand command) {
			var result = await _mediator.Send(command with { Id = id});
			return Ok(ApiResponse<LedgerAccountTypeDto>.Success(result));
		}

		[HttpPatch("{id:guid}/status")]
		public async Task<ActionResult> ToggleActivate(Guid id, [FromBody] bool isActive) {
			var result = await _mediator.Send(new ToggleLedgerAccountTypeStatusCommand(id, isActive));
			return Ok(ApiResponse<bool>.Success(result));
		}

		[HttpDelete("{id:guid}")]
		public async Task<ActionResult> Delete(Guid id) {
			var result = await _mediator.Send(new DeleteLedgerAccountTypeCommand(id));
			return Ok(ApiResponse<bool>.Success(result));
		}	
	}
}
