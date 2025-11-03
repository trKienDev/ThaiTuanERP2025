using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Account.Dtos;
using ThaiTuanERP2025.Application.Account.Permissions;
using ThaiTuanERP2025.Application.Account.Permissions.Commands.Create;

namespace ThaiTuanERP2025.Api.Controllers.Account
{
	[Authorize]
	[Route("api/permission")]
	[ApiController]
	public class PermissionController : ControllerBase
	{
		private readonly IMediator _mediator;
		public PermissionController(IMediator mediator) => _mediator = mediator;

		[HttpPost("new")]
		public async Task<IActionResult> CreatePermission([FromBody] PermissionRequest request, CancellationToken cancellationToken)
		{
			var result = await _mediator.Send(new CreatePermissionCommand(request), cancellationToken);
			return Ok(ApiResponse<Unit>.Success(result));
		}
	}
}
