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
		public async Task<IActionResult> Create(IFormFile file, [FromForm] string module, [FromForm] string entity, CancellationToken cancellationToken) {
			if (file is null || file.Length == 0)
				return BadRequest(ApiResponse<string>.Fail("Không tìm thấy file tải lên"));

			var raw = new RawObject(
				file.FileName,
				file.ContentType,
				file.Length,
				_ => Task.FromResult<Stream>(file.OpenReadStream())
			);

			var objectId = await _mediator.Send(new CreateStoredObjectCommand(raw, module, entity), cancellationToken);
			return Ok(ApiResponse<Guid>.Success(objectId));
		}
	}
}
