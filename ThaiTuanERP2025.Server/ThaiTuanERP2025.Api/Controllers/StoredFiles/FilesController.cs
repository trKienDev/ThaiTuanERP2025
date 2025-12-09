using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Shared;
using ThaiTuanERP2025.Application.Files.Commands;
using ThaiTuanERP2025.Application.Files.Common;
using ThaiTuanERP2025.Application.Files.Queries;

namespace ThaiTuanERP2025.Api.Controllers.StoredFiles
{
	[ApiController]
	[Authorize]
	[Route("api/files")]
	public class FilesController : ControllerBase
	{
		private readonly IMediator _mediator;
		public FilesController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost("upload-single")]
		public async Task<ActionResult<ApiResponse<UploadSingleFileResult>>> UploadSingle(IFormFile file, [FromForm] string module, [FromForm] string entity, [FromForm] string? entityId = null,[FromForm] bool isPublic = false, CancellationToken cancellationToken = default) {
			if(file is null || file.Length == 0) 
				return BadRequest(ApiResponse<UploadSingleFileResult>.Fail("Không tìm thấy file tải lên"));

			var raw = new RawFile(
				file.FileName,
				file.ContentType,
				file.Length,
				_ => Task.FromResult<Stream>(file.OpenReadStream())
			);

			// dùng UploadFileHandler xử lý, build objectKey & lưu DB (StoredFile). :contentReference[oaicite:5]{index=5}
			var response = await _mediator.Send(new UploadSingleFileCommand(raw, module, entity, entityId, isPublic), cancellationToken); 

			return Ok(ApiResponse<UploadSingleFileResult>.Success(response, "Upload file thành công"));
		}

		[HttpPost("upload-multiple")]
		public async Task<ActionResult<ApiResponse<List<UploadSingleFileResult>>>> UploadMultiple(List<IFormFile> files, [FromForm] string module, [FromForm] string entity, [FromForm] string? entityId = null, [FromForm] bool isPublic = false, CancellationToken cancellationToken = default) {
			if (files is null || files.Count == 0)
				return BadRequest(ApiResponse<List<UploadSingleFileResult>>.Fail("Danh sách file tải lên trống"));

			var raws = files.Where(file => file != null && file.Length > 0)
				.Select(file => new RawFile(file.FileName, file.ContentType, file.Length, _ => Task.FromResult<Stream>(file.OpenReadStream())))
				.ToList();

			// loop upload & lưu DB. :contentReference[oaicite:6]{index=6}
			var response = await _mediator.Send(new UploadMultipleFilesCommand(raws, module, entity, entityId, isPublic), cancellationToken);

			return Ok(ApiResponse<List<UploadSingleFileResult>>.Success(response, "Tải lên các files thành công"));
		}

		[HttpGet("{id:guid}/download-url")]
		public async Task<ActionResult<ApiResponse<string>>> GetDownloadUrl(Guid id, CancellationToken cancellationToken) {
			var url = await _mediator.Send(new GetFileDownloadUrlQuery(id), cancellationToken);
			return Ok(ApiResponse<string>.Success(url, "Tạo link tải file thành công"));
		}

		[Authorize]
		[HttpGet("{id:guid}/download")]
		public async Task<IActionResult> Download(Guid id, CancellationToken cancellationToken)
		{
			var dto = await _mediator.Send(new DownloadStoredFileQuery(id), cancellationToken);

			return File(dto.Stream, dto.ContentType, dto.FileName);
		}

		[HttpDelete("{id:guid}")]
		public async Task<ActionResult<ApiResponse<string>>> SoftDelete(Guid id, CancellationToken cancellationToken) {
			await _mediator.Send(new SoftDeleteFileCommand(id), cancellationToken);
			return Ok(ApiResponse<string>.Success("Đã di chuyển file vào thùng rác"));
		} 

		[HttpDelete("{id:guid}/hard")]
		public async Task<ActionResult<ApiResponse<string>>> HardDelete(Guid id, CancellationToken cancellationToken) {
			await _mediator.Send(new HardDeleteCommand(id), cancellationToken);
			return Ok(ApiResponse<string>.Success("Đã xóa vĩnh viễn"));
		} 
	}
}
