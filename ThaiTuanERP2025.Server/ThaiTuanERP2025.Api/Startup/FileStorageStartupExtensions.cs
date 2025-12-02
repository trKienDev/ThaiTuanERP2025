using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using ThaiTuanERP2025.Infrastructure.StoredFiles.Configurations;

namespace ThaiTuanERP2025.Api.Startup
{
	public static class FileStorageStartupExtensions
	{
		/// <summary>
		/// Đảm bảo thư mục lưu file tồn tại, đồng thời cấu hình Static File Middleware.
		/// </summary>
		public static async Task EnsureFileStorageReadyAsync(this WebApplication app)
		{
			// Lấy cấu hình FileStorageOptions từ DI container
			var storageOpt = app.Services.GetRequiredService<IOptions<FileStorageOptions>>().Value;

			// Tạo thư mục nếu chưa có
			Directory.CreateDirectory(storageOpt.BasePath);

			// Cấu hình Static File middleware để phục vụ file public
			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider(storageOpt.BasePath),
				RequestPath = storageOpt.PublicRequestPath
			});

			await Task.CompletedTask;
		}
	}
}
