using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Shared;
using ThaiTuanERP2025.Api.Security;
using ThaiTuanERP2025.Application.Account.Users.Commands;
using ThaiTuanERP2025.Application.Account.Users.Queries;
using ThaiTuanERP2025.Application.Account.Users.Requests;
using ThaiTuanERP2025.Application.Account.Users;

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

		#region GET
		[HttpGet]
		public async Task<IActionResult> GetAllAsync([FromQuery] string? keyword, [FromQuery] string? role, [FromQuery] Guid? departmentId, CancellationToken cancellationToken
		) {
			var users = await _mediator.Send(new GetAllUsersQuery(keyword, role, departmentId), cancellationToken);
			return Ok(ApiResponse<IReadOnlyList<UserInforDto>>.Success(users));
		}

		// Lấy thông tin user hiện tại (yêu cầu đăng nhập)
		[HttpGet("me")]
		public async Task<ActionResult<UserDto>> GetCurrentUser(CancellationToken ct)
		{
			var result = await _mediator.Send(new GetProfilleQuery(User), ct);
			return Ok(ApiResponse<UserDto>.Success(result));
		}

		[HttpGet("{id:guid}/managers/ids")]
		public async Task<IActionResult> GetMangerIds(Guid Id, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new GetUserManagerIdsQuery(Id), cancellationToken);
			return Ok(ApiResponse<IReadOnlyList<Guid>>.Success(result));
		}

		[HttpGet("me/managers")]
		public async Task<IActionResult> GetMyManagers(CancellationToken cancellationToken)
		{
			var dtos = await _mediator.Send(new GetMyManagersQuery(), cancellationToken);
			return Ok(ApiResponse<IReadOnlyList<UserBriefAvatarDto>>.Success(dtos));
		}

		//[HttpGet("me/department/managers")]
		//public async Task<IActionResult> GetDepartmentManagersByUser(CancellationToken cancellationToken) {
		//	var result = await _mediator.Send(new GetDepartmentManagersByUserQuery(), cancellationToken);
		//	return Ok(ApiResponse<IReadOnlyList<UserBriefAvatarDto>>.Success(result));
		//}

		[HttpGet("search")]
		public async Task<IActionResult> Search([FromQuery] string keyword, CancellationToken cancellationToken)
		{
			var dtos = await _mediator.Send(new SearchUserByNameQuery(keyword));
			return Ok(ApiResponse<IReadOnlyList<UserBriefAvatarDto>>.Success(dtos));
		} 
		#endregion

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(command, cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));
		}

		#region PUT
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
		#endregion

		[HasPermission("user.delete")]
		[HttpDelete("{id:guid}")]
		public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken) {
			var result = await _mediator.Send(new DeleteUserCommand(id), cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));
		}
	}
}
