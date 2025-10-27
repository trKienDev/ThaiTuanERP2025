using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Account.Commands.RBAC.AssignPermissionToRole;
using ThaiTuanERP2025.Application.Account.Commands.RBAC.AssignRoleToUser;
using ThaiTuanERP2025.Application.Account.Commands.RBAC.CreateRole;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Account.Queries.Permissions.GetAllPermissions;
using ThaiTuanERP2025.Application.Account.Queries.Roles.GetAllRoles;

namespace ThaiTuanERP2025.Api.Controllers.Account
{
	[Authorize]
	[ApiController]
	[Route("api/rbac")]
	public class RBACController : ControllerBase
	{
		private readonly IMediator _mediator;
		public RBACController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("permissions/all")]
		public async Task<IActionResult> GetAllPermissions()
		{
			var result = await _mediator.Send(new GetAllPermissionsQuery());
			return Ok(ApiResponse<IEnumerable<PermissionDto>>.Success(result));
		}

		[HttpPost("role/assign-to-user")]
		public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleToUserCommand command)
		{
			var result = await _mediator.Send(command);
			return Ok(ApiResponse<Unit>.Success(result));
		}

		[HttpPost("permission/assign-to-role")]
		public async Task<IActionResult> AssignPermissionToRole([FromBody] AssignPermissionToRoleCommand command)
		{
			var result = await _mediator.Send(command);
			return Ok(ApiResponse<Unit>.Success(result));
		}
	}
}
