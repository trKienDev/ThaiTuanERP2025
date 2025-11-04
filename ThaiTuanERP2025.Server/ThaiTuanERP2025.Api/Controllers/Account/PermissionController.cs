using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Account.Permissions;
using ThaiTuanERP2025.Application.Account.Permissions.Commands.AssignRole;
using ThaiTuanERP2025.Application.Account.Permissions.Commands.Create;
using ThaiTuanERP2025.Application.Account.Permissions.Queries.All;
using ThaiTuanERP2025.Application.Account.Permissions.Queries.ByRole;

namespace ThaiTuanERP2025.Api.Controllers.Account
{
	[Authorize]
	[Route("api/permission")]
	[ApiController]
	public class PermissionController : ControllerBase
	{
		private readonly IMediator _mediator;
		public PermissionController(IMediator mediator) => _mediator = mediator;

		[HttpGet("all")]	
		public async Task<IActionResult> GetAllPermissions(CancellationToken cancellationToken)
		{
			var dtos =  await _mediator.Send(new GetAllPermissionsQuery(), cancellationToken);
			return Ok(ApiResponse<IReadOnlyList<PermissionDto>>.Success(dtos));
		}

		[HttpGet("role/{roleId:guid}")]
		public async Task<IActionResult> GetByRole([FromRoute] Guid roleId, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new GetPermissionsByRoleIdQuery(roleId), cancellationToken);
			return Ok(ApiResponse<IReadOnlyList<PermissionDto>>.Success(result));
		}

		[HttpPost("new")]
		public async Task<IActionResult> CreatePermission([FromBody] PermissionRequest request, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new CreatePermissionCommand(request), cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));
		}
	}
}
