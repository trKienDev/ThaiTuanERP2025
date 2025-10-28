using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Api.Contracts.Users;
using ThaiTuanERP2025.Api.Security;
using ThaiTuanERP2025.Application.Account.Commands.Users.CreateUser;
using ThaiTuanERP2025.Application.Account.Commands.Users.SetUserManagers;
using ThaiTuanERP2025.Application.Account.Commands.Users.UpdateUserAvatarFileId;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Account.Queries.Users.GetAllUsers;
using ThaiTuanERP2025.Application.Account.Queries.Users.GetCurrentUser;
using ThaiTuanERP2025.Application.Account.Queries.Users.GetUserById;
using ThaiTuanERP2025.Application.Account.Queries.Users.GetUserManagerIds;
using ThaiTuanERP2025.Application.Account.Queries.Users.GetUserManagers;

namespace ThaiTuanERP2025.Api.Controllers.Account
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class UserController : ControllerBase
	{
		private readonly IMediator _mediator;

		public UserController(IMediator mediator) {
			_mediator = mediator;
		}

		/// <summary>
		/// Lấy thông tin người dùng theo ID
		/// </summary>
		[HttpGet("{id:guid}")]
		public async Task<IActionResult> GetById(Guid id) {
			var user = await _mediator.Send(new GetUserByIdQuery(id));
			return Ok(ApiResponse<UserDto>.Success(user));	
		}

		/// <summary>
		/// Lấy danh sách người dùng có thể lọc theo keyword, role, department
		/// </summary>
		[HttpGet("all")]
		public async Task<IActionResult> GetAll([FromQuery] string? keyword, [FromQuery] string? role, Guid? departmentId) {
			var query = new GetAllUsersQuery(keyword, role, departmentId);
			var result = await _mediator.Send(query);
			return Ok(ApiResponse<List<UserDto>>.Success(result));
		}

		[HttpGet("me")]
		public async Task<IActionResult> GetMe() {
			var user = await _mediator.Send(new GetCurrentUserQuery(User));
			return Ok(ApiResponse<UserDto>.Success(user));
		}

		[HttpGet("{id:guid}/managers/ids")]
		public async Task<IActionResult> GetMangerIds(Guid Id, CancellationToken cancellationToken) {
			var result = await _mediator.Send(new GetUserManagerIdsQuery(Id), cancellationToken);
			return Ok(ApiResponse<List<Guid>>.Success(result));
		}

		[HttpGet("{id:guid}/managers")]
		public async Task<ActionResult<List<UserDto>>> GetManagers(Guid id, CancellationToken cancellationToken) {
			var result = await _mediator.Send(new GetUserManagersQuery(id), cancellationToken);
			return Ok(ApiResponse<List<UserDto>>.Success(result));
		}

		/// <summary>
		/// Tạo người dùng
		/// </summary>
		[HttpPost("new")]
		public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
		{
			var result = await _mediator.Send(command);
			return Ok(ApiResponse<UserDto>.Success(result));
		}

		[HttpPut("{id:guid}/avatar")]
		public async Task<ActionResult<ApiResponse<string>>> SetAvatar(Guid id, [FromBody] SetUserAvatarRequest request, CancellationToken cancellationToken) {
			await _mediator.Send(new UpdateUserAvatarFileIdCommand(id, request.FileId), cancellationToken);
			return Ok(ApiResponse<string>.Success("Cập nhật avatar thành công"));
		}

		[HasPermission("account.set-manager")]
		[HttpPut("{id:guid}/managers")]
		public async Task<IActionResult> SetManagers(Guid id, [FromBody] SetUserManagerRequest request, CancellationToken cancellationToken)
		{
			var command = new SetUserManagersCommand(id, request.ManagerIds ?? new List<Guid>(), request.PrimaryManagerId);
			await _mediator.Send(command, cancellationToken);
			return Ok(ApiResponse<string>.Success("Cập nhật người quản lý thành công"));
		}
	}
}
