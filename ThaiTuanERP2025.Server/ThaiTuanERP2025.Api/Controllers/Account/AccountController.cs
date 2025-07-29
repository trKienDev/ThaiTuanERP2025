using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Account.Commands.ChangePassword;
using ThaiTuanERP2025.Application.Account.Commands.Login;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Account.Queries.GetCurrentuser;

namespace ThaiTuanERP2025.Api.Controllers.Account
{
	[ApiController]
	[Route("api/[controller]")]
	public class AccountController : ControllerBase
	{
		private readonly IMediator _mediator;
		
		public AccountController(IMediator mediator) {
			_mediator = mediator;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginCommand command) {
			var result = await _mediator.Send(command);
			return Ok(ApiResponse<LoginResultDto>.Success(result));
		}

		[Authorize]
		[HttpGet("me")]
		public async Task<IActionResult> GetCurrentUser() {
			var query = new GetCurrentuserQuery(User); // lấy từ JWT Claims
			var result = await _mediator.Send(query);
			return Ok(ApiResponse<UserDto>.Success(result));
		}

		[Authorize]
		[HttpPut("change-password")]
		public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command) {
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if(!Guid.TryParse(userId, out Guid id)) 
				return Unauthorized(ApiResponse<string>.Fail("Token không hợp lệ"));

			command.UserId = id;
			var result = await _mediator.Send(command);
			return Ok(ApiResponse<string>.Success(result));
		}
	}
}
