using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Infrastructure.StoredFiles.Configurations
{
	public sealed class FileStorageOptions
	{
		public string Bucket { get; set; } = "files";
		public int PresignedExpirySeconds { get; set; } = 300;
		public string? BasePath { get; set; } = "D:\\ERP-Files";
	}
}
