using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Account.Users.Queries.Profile;
using ThaiTuanERP2025.Application.Authentication.DTOs;
using ThaiTuanERP2025.Application.Exceptions;

namespace ThaiTuanERP2025.Api.Controllers.Account
{
	[ApiController]
	[Route("api/user")]
	public class UserController : ControllerBase
	{
		private readonly IMediator _mediator;
		
		public UserController(IMediator mediator) {
			_mediator = mediator;
		}

		/// Lấy thông tin user hiện tại (yêu cầu đăng nhập)
		[HttpGet("me")]
		[Authorize] 
		public async Task<ActionResult<UserDto>> GetCurrentUser(CancellationToken ct)
		{
			try
			{
				var result = await _mediator.Send(new GetProfilleQuery(User), ct);
				return Ok(ApiResponse<UserDto>.Success(result));
			}
			catch (NotFoundException ex)
			{
				return NotFound(new { message = ex.Message });
			}
			catch (AppException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = $"Lỗi hệ thống: {ex.Message}" });
			}
		}
	}
}
