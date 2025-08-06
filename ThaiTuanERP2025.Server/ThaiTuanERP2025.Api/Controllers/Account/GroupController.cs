using MediatR;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Account.Commands.AddUserToGroup;
using ThaiTuanERP2025.Application.Account.Commands.ChangeGroupAdmin;
using ThaiTuanERP2025.Application.Account.Commands.DeleteGroup;
using ThaiTuanERP2025.Application.Account.Commands.Group.CreateGroup;
using ThaiTuanERP2025.Application.Account.Commands.Groups.AddUserToGroup;
using ThaiTuanERP2025.Application.Account.Commands.RemoveUserFromGroup;
using ThaiTuanERP2025.Application.Account.Commands.UpdateGroup;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Account.Queries.GetAllGroupsQuery;
using ThaiTuanERP2025.Application.Account.Validators;

namespace ThaiTuanERP2025.Api.Controllers.Account
{
	[Route("api/[controller]")]	
	[ApiController]
	public class GroupController : ControllerBase
	{
		private readonly IMediator _mediator;	
		public GroupController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreateGroupCommand command) {
			var group = await _mediator.Send(command);
			return Ok(ApiResponse<GroupDto>.Success(group, "Tạo nhóm thành công"));
		}

		[HttpPut("{groupId}/add-user")]
		public async Task<IActionResult> AddUser(Guid groupId, [FromBody] AddUserToGroupDto dto) {
			await _mediator.Send(new AddUserToGroupCommand(groupId, dto.UserId));
			return Ok(ApiResponse<object>.Success(null!, "Thêm người dùng vào nhóm thành công"));
		}

		[HttpPut("{groupId}/remove-user")]
		public async Task<IActionResult> RemoveUser(Guid groupId, [FromBody] RemoveUserDto dto) {
			await _mediator.Send(new RemoveUserFromGroupCommand(groupId, dto.UserId, dto.RequestorId));
			return Ok(ApiResponse<object>.Success(null!, "Xóa người dùng khỏi nhóm thành công"));	
		}

		[HttpPut("{groupId}/change-admin")]
		public async Task<IActionResult> ChangeAdmin(Guid groupId, [FromBody] ChangeAdminDto dto) {
			await _mediator.Send(new ChangeGroupAdminCommand(groupId, dto.NewAdminId, dto.RequestorId));
			return Ok(ApiResponse<string>.Success(null!, "Thay đổi quản trị viên nhóm thành công"));
		}

		[HttpPut("{groupId}")]
		public async Task<IActionResult> Update(Guid groupId, UpdateGroupDto dto) {
			await _mediator.Send(new UpdateGroupCommand(groupId, dto.Name, dto.Description, dto.RequestorId));
			return Ok(ApiResponse<string>.Success(null!, "Cập nhật nhóm thành công"));
		}

		[HttpDelete("{groupId}")]
		public async Task<IActionResult> Delete(Guid groupId, [FromQuery] Guid requestorId) {
			await _mediator.Send(new DeleteGroupCommand(groupId, requestorId));
			return Ok(ApiResponse<string>.Success(null!, "Xóa nhóm thành công"));
		}

		[HttpGet]
		public async Task<IActionResult> GetAll() {
			var groups = await _mediator.Send(new GetAllGroupsQuery());
			return Ok(ApiResponse<List<GroupDto>>.Success(groups));
		}
	}
}
