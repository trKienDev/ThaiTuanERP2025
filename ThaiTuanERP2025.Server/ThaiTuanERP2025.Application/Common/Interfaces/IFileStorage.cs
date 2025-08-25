using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Common.Interfaces
{
	public interface IFileStorage
	{
		Task EnsureBucketAsync(string bucket, CancellationToken cancellationToken);
		Task UploadAsync(string bucket, string objectKey, Stream content, string contentType, CancellationToken cancellationToken);
		Task<string> GetPresignedGetUrlAsync(string bucket, string objectKey, TimeSpan expiry, CancellationToken cancellationToken);
	}
}
