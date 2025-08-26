using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Api.Contracts.Users;
using ThaiTuanERP2025.Application.Account.Commands.Users.CreateUser;
using ThaiTuanERP2025.Application.Account.Commands.Users.UpdateUser;
using ThaiTuanERP2025.Application.Account.Commands.Users.UpdateUserAvatarFileId;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Account.Queries.Users.GetAllUsers;
using ThaiTuanERP2025.Application.Account.Queries.Users.GetCurrentUser;
using ThaiTuanERP2025.Application.Account.Queries.Users.GetUserById;

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

		/// <summary>
		/// Tạo người dùng
		/// </summary>
		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
		{
			var result = await _mediator.Send(command);
			return Ok(ApiResponse<UserDto>.Success(result));
		}

		[Authorize(Roles = "Admin")]
		[HttpPut("{id:guid}")]
		public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserCommand command) {
			if(id != command.Id) return BadRequest(ApiResponse<string>.Fail("ID không khớp"));
			var result = await _mediator.Send(command);
			return Ok(ApiResponse<UserDto>.Success(result));
		}

		[HttpPut("{id:guid}/avatar")]
		public async Task<ActionResult<ApiResponse<string>>> SetAvatar(Guid id, [FromBody] SetUserAvatarRequest request, CancellationToken cancellationToken) {
			await _mediator.Send(new UpdateUserAvatarFileIdCommand(id, request.FileId), cancellationToken);
			return Ok(ApiResponse<string>.Success("Cập nhật avatar thành công"));
		}
	}
}
