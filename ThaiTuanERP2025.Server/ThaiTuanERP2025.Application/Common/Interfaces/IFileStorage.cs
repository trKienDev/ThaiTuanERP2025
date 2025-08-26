using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Common.Interfaces
{
	public interface IFileStorage
	{
		Task EnsureReadyAsync(CancellationToken cancellationToken);
		Task UploadAsync(string objectKey, Stream content, string contentType, CancellationToken cancellationToken);
		Task<string> GetPresignedGetUrlAsync(string objectKey, CancellationToken cancellationToken);
		Task RemoveAsync(string objectKey, CancellationToken cancellationToken);
	}
}
