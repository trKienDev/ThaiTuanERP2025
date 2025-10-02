using Microsoft.AspNetCore.Mvc;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Infrastructure.Authentication;

namespace ThaiTuanERP2025.Api.Controllers.FileStorage
{
	[ApiController]
	[Route("api/files")]
	public class FilesController : ControllerBase
	{
		private readonly IFileStorage _storage;
		private readonly IConfiguration _cfg;
		private readonly ICurrentUserService _currentUserService;
		public FilesController(IFileStorage storage, IConfiguration cfg, CurrentUserService currentUserService) {
			_storage = storage;
			_cfg = cfg;
			_currentUserService = currentUserService;
		}

		[HttpPost("upload")]
		public async Task<ActionResult<object>> Upload(IFormFile file, CancellationToken cancellationToken) {
			if (file is null || file.Length == 0)
				return BadRequest(ApiResponse<object>.Fail("Hệ thống không nhận được file upload nào"));

			await _storage.EnsureReadyAsync(cancellationToken);

			var userId = _currentUserService.UserId ?? Guid.Empty;
			var objectKey = $"tenants/{userId}/expense/invoices/{DateTime.UtcNow:yyyy/MM}/{Guid.NewGuid():N}{Path.GetExtension(file.FileName)}";
			await using var stream = file.OpenReadStream();
			await _storage.UploadAsync(objectKey, stream, file.ContentType, cancellationToken);

			var result = new { objectKey, fileName = file.FileName, size = file.Length };
			return Ok(ApiResponse<object>.Success(result, "Upload file thành công"));
		}

		[HttpGet("download-url")]
		public async Task<ActionResult<ApiResponse<string>>> GetDownloadUrl([FromQuery] string bucket, [FromQuery] string objectKey, CancellationToken cancellationToken)
		{
			if (string.IsNullOrWhiteSpace(bucket) || string.IsNullOrWhiteSpace(objectKey))
				return BadRequest(ApiResponse<string>.Fail("Thiếu tham số bucket hoặc objectKey"));

			var seconds = int.Parse(_cfg["Minio:PresignedExpirySeconds"] ?? "300");
			var url = await _storage.GetPresignedGetUrlAsync(objectKey, cancellationToken);

			return Ok(ApiResponse<string>.Success(url, "Tạo link tải file thành công"));
		}



	}
}
