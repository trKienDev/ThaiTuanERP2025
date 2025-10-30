using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Account.Commands.Permissions.AssignPermissionToRole;
using ThaiTuanERP2025.Application.Account.Commands.Permissions.CreateNewPermission;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Account.Queries.Permissions.GetAllPermissions;
using ThaiTuanERP2025.Application.Account.Queries.Permissions.GetPermissionsByRoleId;

namespace ThaiTuanERP2025.Api.Controllers.Account
{
	[Authorize]
	[ApiController]
	[Route("api/permission")]
	public class PermissionController :ControllerBase
	{
		private readonly IMediator _mediator;
		public PermissionController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("all")]
		public async Task<IActionResult> GetAllPermissions()
		{
			var result = await _mediator.Send(new GetAllPermissionsQuery());
			return Ok(ApiResponse<IEnumerable<PermissionDto>>.Success(result));
		}

		[HttpGet("by-role-id/{roleId:guid}")]
		public async Task<IActionResult>  GetByRoleId([FromRoute] Guid roleId)
		{
			var result = await _mediator.Send(new GetPermissionsByRoleIdQuery(roleId));
			return Ok(ApiResponse<IEnumerable<PermissionDto>>.Success(result));
		}

		[HttpPost("new")]
		public async Task<IActionResult> Create(PermissionRequest request) {
			var result = await _mediator.Send(new CreateNewPermissionCommand(request));
			return Ok(ApiResponse<Unit>.Success(result));
		}

		[HttpPost("{id:guid}/assign-permissions")]
		public async Task<IActionResult> AssignPermissionToRole(Guid id, [FromBody] List<Guid> permissionIds, CancellationToken cancellationToken)
		{
			var command = new AssignPermissionToRoleCommand(id, permissionIds);
			var result = await _mediator.Send(command, cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));
		}
	}
}
