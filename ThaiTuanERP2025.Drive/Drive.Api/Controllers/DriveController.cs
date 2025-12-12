using Drive.Api.Contracts;
using Drive.Application.Commands;
using Drive.Application.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Drive.Api.Controllers
{
	[ApiController]
	[Route("api/drive")]
	public class DriveController : ControllerBase
	{
		private readonly IMediator _mediator;
		public DriveController(IMediator mediator) {
			_mediator = mediator;
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] StoredObjectPayload payload, CancellationToken cancellationToken) {
			if (payload is null)
				return BadRequest(ApiResponse<string>.Fail("Dữ liệu không hợp lệ"));

			var objectId = await _mediator.Send(new CreateStoredObjectCommand(payload), cancellationToken);
			return Ok(ApiResponse<Guid>.Success(objectId));
		}
	}
}
