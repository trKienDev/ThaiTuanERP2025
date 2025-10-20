using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Presentation.Common;
using ThaiTuanERP2025.Application.Files.Commands.HardDelete;
using ThaiTuanERP2025.Application.Files.Commands.SoftDeleteFile;
using ThaiTuanERP2025.Application.Files.Commands.UploadFile;
using ThaiTuanERP2025.Application.Files.Commands.UploadMultipleFiles;
using ThaiTuanERP2025.Application.Files.Common;
using ThaiTuanERP2025.Application.Files.Queries.GetFileDownloadUrl;

namespace ThaiTuanERP2025.Presentation.Controllers.Files
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
		public async Task<ActionResult<ApiResponse<UploadFileResult>>> UploadSingle(IFormFile file, [FromForm] string module, [FromForm] string entity, [FromForm] string? entityId = null,[FromForm] bool isPublic = false, CancellationToken cancellationToken = default) {
			if(file is null || file.Length == 0) 
				return BadRequest(ApiResponse<UploadFileResult>.Fail("Không tìm thấy file tải lên"));

			var raw = new RawFile(
				file.FileName,
				file.ContentType,
				file.Length,
				_ => Task.FromResult<Stream>(file.OpenReadStream())
			);

			// dùng UploadFileHandler xử lý, build objectKey & lưu DB (StoredFile). :contentReference[oaicite:5]{index=5}
			var response = await _mediator.Send(new UploadFileCommand(raw, module, entity, entityId, isPublic), cancellationToken); 

			return Ok(ApiResponse<UploadFileResult>.Success(response, "Upload file thành công"));
		}

		[HttpPost("upload-multiple")]
		public async Task<ActionResult<ApiResponse<List<UploadFileResult>>>> UploadMultiple(List<IFormFile> files, [FromForm] string module, [FromForm] string entity, [FromForm] string? entityId = null, [FromForm] bool isPublic = false, CancellationToken cancellationToken = default) {
			if (files is null || files.Count == 0)
				return BadRequest(ApiResponse<List<UploadFileResult>>.Fail("Danh sách file tải lên trống"));

			var raws = files.Where(file => file != null && file.Length > 0)
				.Select(file => new RawFile(file.FileName, file.ContentType, file.Length, _ => Task.FromResult<Stream>(file.OpenReadStream())))
				.ToList();

			// loop upload & lưu DB. :contentReference[oaicite:6]{index=6}
			var response = await _mediator.Send(new UploadMultipleFilesCommand(raws, module, entity, entityId, isPublic), cancellationToken);

			return Ok(ApiResponse<List<UploadFileResult>>.Success(response, "Tải lên các files thành công"));
		}

		[HttpGet("{id:guid}/download-url")]
		public async Task<ActionResult<ApiResponse<string>>> GetDownloadUrl(Guid id, CancellationToken cancellationToken) {
			var url = await _mediator.Send(new GetFileDownloadUrlQuery(id), cancellationToken);
			return Ok(ApiResponse<string>.Success(url, "Tạo link tải file thành công"));
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
