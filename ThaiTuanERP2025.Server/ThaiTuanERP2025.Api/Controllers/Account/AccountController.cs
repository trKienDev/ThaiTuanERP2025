using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Account.Commands.Login;
using ThaiTuanERP2025.Application.Account.Dtos;

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

		/// <summary>
		/// Đăng nhập vào hệ thống ERP.
		/// </summary>
		/// <param name="command">Thông tin đăng nhập (EmployeeCode + Password)</param>
		/// <returns>JWT Token + Thông tin cơ bản của user</returns>
		[AllowAnonymous]
		[HttpPost("login")]
		[ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> Login([FromBody] LoginCommand command)
		{
			var result = await _mediator.Send(command);
			return Ok(ApiResponse<LoginResponseDto>.Success(result));
		}

		/// <summary>
		/// Lấy thông tin user hiện tại từ JWT token.
		/// </summary>
		[Authorize]
		[HttpGet("me")]
		[ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
		public IActionResult GetCurrentUser()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var username = User.FindFirstValue(ClaimTypes.Name);
			var email = User.FindFirstValue(ClaimTypes.Email);
			var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
			var permissions = User.FindAll("permission").Select(c => c.Value).ToList();

			var response = new
			{
				Id = userId,
				Username = username,
				Email = email,
				Roles = roles,
				Permissions = permissions
			};

			return Ok(ApiResponse<object>.Success(response));
		}
	}
}
