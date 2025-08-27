using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Infrastructure.StoredFiles.Configurations
{
	public sealed class MinioSettings 
	{
		public string Endpoint { get; set; } = default!;
		public string AccessKey { get; set; } = default!;
		public string SecretKey { get; set; } = default!;
		public bool UseSSL { get; set; } = false;
	}
}
