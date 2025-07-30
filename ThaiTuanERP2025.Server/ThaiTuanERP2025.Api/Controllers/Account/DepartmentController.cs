using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Application.Account.Commands.AddDepartment;

namespace ThaiTuanERP2025.Api.Controllers.Account
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class DepartmentController : ControllerBase
	{
		private readonly IMediator _mediator;
		public DepartmentController(IMediator mediator)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] AddDepartmentCommand command)
		{
			if (command == null) return BadRequest("Command cannot be null");
			var departmentId = await _mediator.Send(command);
			return Ok(new { departmentId });
		}
	}
}
