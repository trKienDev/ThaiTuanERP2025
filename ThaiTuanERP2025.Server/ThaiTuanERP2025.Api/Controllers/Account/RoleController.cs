using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Account.Permissions.Commands;
using ThaiTuanERP2025.Application.Account.Roles;
using ThaiTuanERP2025.Application.Account.Roles.Commands;
using ThaiTuanERP2025.Application.Account.Roles.Queries;
using ThaiTuanERP2025.Application.Account.Roles.Request;

namespace ThaiTuanERP2025.Api.Controllers.Account
{
	[ApiController]
	[Route("api/role")]
	[Authorize]
	public class RoleController : ControllerBase
	{
		private readonly IMediator _mediator;
		public RoleController(IMediator mediator) {
			_mediator = mediator;
		}

		[HttpGet("all")]
		public async Task<IActionResult> GetAllRolesAsync(CancellationToken cancellationToken)
		{
			var roles = await _mediator.Send(new GetAllRolesQuery(), cancellationToken);
			return Ok(ApiResponse<IEnumerable<RoleDto>>.Success(roles));
		}

		[HttpPost("new")]
		public async Task<IActionResult> CreateRole([FromBody] RoleRequest request, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new CreateRoleCommand(request), cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));
		}

		[HttpPost("{roleId:guid}/permissions")]
		public async Task<IActionResult> AssignPermissions([FromRoute] Guid roleId, [FromBody] List<Guid> permissionIds, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new AssignPermissionToRoleCommand(roleId, permissionIds), cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));
		}
	}
}
