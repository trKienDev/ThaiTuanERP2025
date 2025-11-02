using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Authentication.Commands.Login;
using ThaiTuanERP2025.Application.Authentication.Commands.RefreshAccessToken;
using ThaiTuanERP2025.Application.Authentication.Commands.RevokeRefreshToken;
using ThaiTuanERP2025.Application.Authentication.DTOs;

namespace ThaiTuanERP2025.Api.Controllers.Authentications
{
	[Route("api/auth")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IMediator _mediator;

		public AuthController(IMediator mediator) => _mediator = mediator;

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

		// POST api/auth/refresh
		[HttpPost("refresh-token")]
		[AllowAnonymous]
		public async Task<IActionResult> Refresh([FromBody] RefreshRequest request, CancellationToken ct)
		{
			var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
			var result = await _mediator.Send(new RefreshAccessTokenCommand(request.RefreshToken, ip), ct);
			return Ok(result);
		}

		// POST api/auth/logout
		[HttpPost("logout")]
		[Authorize]
		public async Task<IActionResult> Logout([FromBody] RefreshRequest request, CancellationToken ct)
		{
			var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
			await _mediator.Send(new RevokeRefreshTokenCommand(request.RefreshToken, ip), ct);
			return NoContent();
		}

		public sealed record RefreshRequest(string RefreshToken);
	}
}
