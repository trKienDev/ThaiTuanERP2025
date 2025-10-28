using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sprache;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Account.Commands.Roles.AssignRoleToUser;
using ThaiTuanERP2025.Application.Account.Commands.Roles.CreateRole;
using ThaiTuanERP2025.Application.Account.Commands.Roles.DeleteRole;
using ThaiTuanERP2025.Application.Account.Commands.Roles.ToggleRoleActive;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Account.Queries.Roles.GetAllRoles;

namespace ThaiTuanERP2025.Api.Controllers.Account
{
	[ApiController]
	[Authorize]
	[Route("api/roles")]
	public class RoleController : ControllerBase
	{
		private readonly IMediator _mediator;
		public RoleController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("all")]
		public async Task<IActionResult> GetAllRoles()
		{
			var result = await _mediator.Send(new GetAllRolesQuery());
			return Ok(ApiResponse<IEnumerable<RoleDto>>.Success(result));
		}

		[HttpPost("new")]
		public async Task<IActionResult> CreateRole([FromBody] RoleRequest request)
		{
			var result = await _mediator.Send(new CreateRoleCommand(request));
			return Ok(ApiResponse<Unit>.Success(result));
		}

		[HttpPost("assign-to-user")]
		public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleToUserCommand command)
		{
			var result = await _mediator.Send(command);
			return Ok(ApiResponse<Unit>.Success(result));
		}

		[HttpPatch("{roleId:guid}/toggle-activate")]
		public async Task<IActionResult> ToogleActive(Guid roleId, CancellationToken cancellationToken) {
			var result = await _mediator.Send(new ToggleRoleActiveCommand(roleId), cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));
		}

		[HttpDelete("{roleId:guid}")]
		public async Task<IActionResult> Delete(Guid roleId, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new DeleteRoleCommand(roleId), cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));
		}
	}
}
