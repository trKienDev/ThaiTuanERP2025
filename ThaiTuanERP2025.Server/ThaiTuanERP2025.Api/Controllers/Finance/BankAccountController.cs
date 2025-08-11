using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Common.Models;
using ThaiTuanERP2025.Application.Finance.Commands.BankAccounts.CreateBankAccount;
using ThaiTuanERP2025.Application.Finance.Commands.BankAccounts.DeleteBankAccount;
using ThaiTuanERP2025.Application.Finance.Commands.BankAccounts.ToggleBankAccountStatus;
using ThaiTuanERP2025.Application.Finance.Commands.BankAccounts.UpdateBankAccount;
using ThaiTuanERP2025.Application.Finance.DTOs;
using ThaiTuanERP2025.Application.Finance.Queries.BankAccounts.GetAllBankAccounts;
using ThaiTuanERP2025.Application.Finance.Queries.BankAccounts.GetBankAccountById;

namespace ThaiTuanERP2025.Api.Controllers.Finance
{
	[Authorize]
	[ApiController]
	[Route("api/bank-accounts")]
	public class BankAccountController : ControllerBase
	{
		private readonly IMediator _mediator;
		public BankAccountController(IMediator mediator)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
		}

		[HttpGet("all")]
		public async Task<IActionResult> GetAll([FromQuery] bool? onlyActive, [FromQuery] Guid? departmentId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
		{
			var data = await _mediator.Send(new GetAllBankAccountsQuery(onlyActive, departmentId, page, pageSize));
			return Ok(ApiResponse<PagedResult<BankAccountDto>>.Success(data, "Lấy danh sách tài khoản ngân hàng thành công"));
		}
		
		[HttpGet("{id:guid}")]
		public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
		{
			var data = await _mediator.Send(new GetBankAccountByIdQuery(id), cancellationToken);
			return Ok(ApiResponse<BankAccountDto>.Success(data, "Lấy thông tin tài khoản ngân hàng thành công"));
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateBankAccountCommand command)
		{
			var data = await _mediator.Send(command);
			return Ok(ApiResponse<BankAccountDto>.Success(data, "Tạo tài khoản ngân hàng thành công"));
		}

		[HttpPut("{id:guid}")]	
		public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBankAccountCommand body)
		{
			if (id != body.Id)
				return BadRequest(ApiResponse<string>.Fail("Id không hợp lệ"));
			var data = await _mediator.Send(body);
			return Ok(ApiResponse<BankAccountDto>.Success(data, "Cập nhật tài khoản ngân hàng thành công"));
		}

		[HttpPatch("{id:guid}/status")]
		public async Task<IActionResult> ToggleStatus(Guid id, [FromQuery] bool isActive)
		{
			await _mediator.Send(new ToggleBankAccountStatusCommand(id, isActive));
			return Ok(ApiResponse<string>.Success("Đã cập nhật hiệu lực"));
		}

		[HttpDelete("{id:guid}")]
		public async Task<IActionResult> Delete(Guid id) { 
			await _mediator.Send(new DeleteBankAccountCommand(id));
			return Ok(ApiResponse<string>.Success("Xóa tài khoản ngân hàng thành công"));
		}

	}
}
