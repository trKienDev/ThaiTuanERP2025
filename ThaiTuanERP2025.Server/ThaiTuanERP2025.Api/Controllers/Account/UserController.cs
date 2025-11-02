using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Account.Users;
using ThaiTuanERP2025.Application.Account.Users.Queries.All;
using ThaiTuanERP2025.Application.Account.Users.Queries.Profile;

namespace ThaiTuanERP2025.Api.Controllers.Account
{
	[ApiController]
	[Route("api/user")]
	[Authorize]
	public class UserController : ControllerBase
	{
		private readonly IMediator _mediator;
		
		public UserController(IMediator mediator) {
			_mediator = mediator;
		}

		/// Lấy thông tin user hiện tại (yêu cầu đăng nhập)
		[HttpGet("me")]
		[Produces("application/json")]
		[ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
		public async Task<ActionResult<UserDto>> GetCurrentUser(CancellationToken ct)
		{
			var result = await _mediator.Send(new GetProfilleQuery(User), ct);
			return Ok(ApiResponse<UserDto>.Success(result));
		}

		[HttpGet("all")]
		[ProducesResponseType(typeof(IReadOnlyList<UserDto>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAllAsync(
			[FromQuery] string? keyword,
			[FromQuery] string? role,
			[FromQuery] Guid? departmentId,
			CancellationToken cancellationToken
		) {
			var users = await _mediator.Send(new GetAllUsersQuery(keyword, role, departmentId), cancellationToken);
			return Ok(ApiResponse<IReadOnlyList<UserDto>>.Success(users));
		}
	}
}
