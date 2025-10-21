using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Application.Expense.Queries.OutgoingBankAccounts.GetAllOutgoingBankAccount;

namespace ThaiTuanERP2025.Api.Controllers.Expense
{
	[ApiController]
	[Authorize]
	[Route("api/outgoing-bank-accounts")]
	public class OutgoingBankAccountController : ControllerBase
	{
		private readonly IMediator _mediator;
		public OutgoingBankAccountController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("all")] 
		public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new GetAllOutgoingBankAccountQuery(), cancellationToken);
			return Ok(ApiResponse<IReadOnlyList<OutgoingBankAccountDto>>.Success(result, "Lấy danh sách tài khoản tiền ra thành công"));
		}

		[HttpPost]
		public async Task<ActionResult> Create([FromBody] OutgoingBankAccountRequest body, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new Application.Expense.Commands.OutgoingBankAccounts.NewOutgoingBankAccount.NewOutgoingBankAccountCommand(body), cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result, "Tạo tài khoản tiền ra thành công"));
		}
	}
}
