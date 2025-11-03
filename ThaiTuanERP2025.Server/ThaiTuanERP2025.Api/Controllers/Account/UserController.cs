using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Account.Users;
using ThaiTuanERP2025.Application.Account.Users.Commands.Create;
using ThaiTuanERP2025.Application.Account.Users.Commands.SetAvatar;
using ThaiTuanERP2025.Application.Account.Users.Commands.SetManager;
using ThaiTuanERP2025.Application.Account.Users.Queries.All;
using ThaiTuanERP2025.Application.Account.Users.Queries.Profile;
using ThaiTuanERP2025.Application.Account.Users.Requests;

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

		[HttpPost("new")]
		public async Task<IActionResult> Create([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(command, cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));
		}


		[HttpPut("{id:guid}/managers")]
		public async Task<IActionResult> SetManagers(Guid id, [FromBody] SetUserManagersRequest request, CancellationToken cancellationToken)
		{
			var command = new SetManagerCommand(id, request.ManagerIds ?? new List<Guid>(), request.PrimaryManagerId);
			await _mediator.Send(command, cancellationToken);
			return Ok(ApiResponse<string>.Success("Cập nhật người quản lý thành công"));
		}

		[HttpPut("{id:guid}/avatar")]
		public async Task<IActionResult> SetAvatar(Guid id, [FromBody] SetUserAvatarRequest request, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new SetUserAvatarCommand(id, request), cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));
		}
	}
}
