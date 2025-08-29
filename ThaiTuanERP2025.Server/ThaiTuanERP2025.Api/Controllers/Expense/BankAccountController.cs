using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Expense.Commands.BankAccounts.CreateSupplierBankAccount;
using ThaiTuanERP2025.Application.Expense.Commands.BankAccounts.CreateUserBankAccount;
using ThaiTuanERP2025.Application.Expense.Commands.BankAccounts.DeleteBankAccount;
using ThaiTuanERP2025.Application.Expense.Commands.BankAccounts.UpdateBankAccount;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Application.Expense.Queries.BankAccounts.GetBankAccountById;
using ThaiTuanERP2025.Application.Expense.Queries.BankAccounts.GetUserBankAccount;
using ThaiTuanERP2025.Application.Expense.Queries.BankAccounts.ListSupplierBankAccount;

namespace ThaiTuanERP2025.Api.Controllers.Expense
{
	[Authorize]
	[ApiController]
	[Route("api/bank-accounts")]
	public class BankAccountController : ControllerBase
	{
		private readonly IMediator _mediator;
		public BankAccountController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("{id:guid}")]
		public async Task<ActionResult<BankAccountDto>> GetById(Guid id, CancellationToken cancellationToken) {
			var result = await _mediator.Send(new GetBankAccountByIdQuery(id), cancellationToken);
			return Ok(ApiResponse<BankAccountDto>.Success(result));	
		}

		// User side (1-1)
		[HttpGet("user/{userId:guid}")]
		public async Task<ActionResult<BankAccountDto?>> GetByUser(Guid userId, CancellationToken cancellationToken) {
			var result = await _mediator.Send(new GetUserBankAccountQuery(userId), cancellationToken);
			return Ok(ApiResponse<BankAccountDto?>.Success(result));
		}

		[HttpPost("user")]
		public async Task<ActionResult<BankAccountDto>> CreateForUser([FromBody] CreateUserBankAccountRequest body, CancellationToken cancellationToken) {
			var result = await _mediator.Send(new CreateUserBankAccountCommand(body), cancellationToken);
			return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<BankAccountDto>.Success(result));
		}

		// SUPPLIER side (1-N)
		[HttpGet("supplier/{supplierId:guid}")]
		public async Task<ActionResult<IReadOnlyList<BankAccountDto>>> ListBySupplier(Guid supplierId, CancellationToken cancellationToken) {
			var result = await _mediator.Send(new ListSupplierBankAccountQuery(supplierId), cancellationToken);
			return Ok(ApiResponse<IReadOnlyList<BankAccountDto>>.Success(result));
		}

		[HttpPost("supplier")]
		public async Task<ActionResult<BankAccountDto>> CreateForSupplier([FromBody] CreateSupplierBankAccountRequest body, CancellationToken cancellationToken) {
			var result = await _mediator.Send(new CreateSupplierBankAccountCommand(body), cancellationToken);
			return Ok(ApiResponse<BankAccountDto>.Success(result));
		}

		// common
		[HttpPut("{id:guid}")]
		public async Task<ActionResult<BankAccountDto>> Update(Guid id, [FromBody] UpdateBankAccountRequest body, CancellationToken cancellationToken) {
			var result = await _mediator.Send(new UpdateBankAccountCommand(id, body), cancellationToken);
			return Ok(ApiResponse<BankAccountDto>.Success(result));
		}

		[HttpDelete("{id:guid}")]
		public async Task<ActionResult<bool>> Delete(Guid id, CancellationToken cancellationToken) {
			await _mediator.Send(new DeleteBankAccountCommand(id), cancellationToken);	
			return Ok(ApiResponse<bool>.Success(true));
		}
	}
}
