using MediatR;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
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
		public async Task<IActionResult<List<LedgerAccountDto>> GetAll() {
			var dto = await _mediator.Send(new GetAllLedgerAccountsQuery());
			if(dto == null) {
				return NotFound(ApiResponse<string>.Fail("Không tìm thấy bất kỳ tài khoản kế toán nào"));
			}
			return Ok(ApiResponse<LedgerAccountDto>.Success(dto));
		} 
	}
}
