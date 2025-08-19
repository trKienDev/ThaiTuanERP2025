using MediatR;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Finance.Commands.BankAccounts.ToggleBankAccountStatus;
using ThaiTuanERP2025.Application.Finance.Commands.LedgerAccounts.CreateLedgerAccount;
using ThaiTuanERP2025.Application.Finance.Commands.LedgerAccounts.DeleteLedgerAccount;
using ThaiTuanERP2025.Application.Finance.Commands.LedgerAccounts.UpdateLedgerAccount;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Application.Finance.Queries.LedgerAccounts.GetAllLedgerAccounts;
using ThaiTuanERP2025.Application.Finance.Queries.LedgerAccounts.GetLedgerAccountById;

namespace ThaiTuanERP2025.Api.Controllers.Finance
{
	[ApiController]
	[Route("api/ledger-accounts")]
	public class LedgerAccountController : ControllerBase
	{
		private readonly IMediator _mediator;
		public LedgerAccountController(IMediator mediator) {
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll() {
			var dto = await _mediator.Send(new GetAllLedgerAccountsQuery());
			return Ok(ApiResponse<List<LedgerAccountDto>>.Success(dto));
		} 

		[HttpGet("{id:guid}")]
		public async Task<IActionResult> GetById(Guid id) {
			var data = await _mediator.Send(new GetLedgerAccountByIdQuery(id));
			return Ok(ApiResponse<LedgerAccountDto>.Success(data));
		}

		[HttpPost]
		public async Task<ActionResult> Create([FromBody] CreateLedgerAccountCommand command) {
			var result = await _mediator.Send(command);
			return Ok(ApiResponse<LedgerAccountDto>.Success(result));
		}

		[HttpPut("{id:guid}")]
		public async Task<IActionResult> Update(Guid id, [FromBody] UpdateLedgerAccountCommand command) {
			var result = await _mediator.Send(command with { Id = id});
			return Ok(ApiResponse<LedgerAccountDto>.Success(result, "Cập nhật thành công"));
		}

		[HttpPatch("{id:guid}/status")]
		public async Task<IActionResult> ToggleStatus(Guid id, [FromBody] bool IsActive) {
			await _mediator.Send(new ToggleBankAccountStatusCommand(id, IsActive));
			return Ok(ApiResponse<bool>.Success(true));
		}

		[HttpDelete("{id:guid}")]
		public async Task<ActionResult> Delete(Guid id) {
			await _mediator.Send(new DeleteLedgerAccountCommand(id));
			return Ok(ApiResponse<bool>.Success(true));
		}
	}
}
