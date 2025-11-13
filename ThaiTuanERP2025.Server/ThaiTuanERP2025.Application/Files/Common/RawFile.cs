using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Files.Shared
{
	public sealed record RawFile(string FileName, string? ContentType, long Length, 
		Func<CancellationToken, Task<Stream>> OpenReadStream // ko phụ thuộc AP.NET Core HTTP --> dễ mock test về sau
	);
}
