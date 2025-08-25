using Minio;
using Minio.DataModel.Args;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ThaiTuanERP2025.Application.Common.Interfaces;

namespace ThaiTuanERP2025.Infrastructure.FileStorage
{
	public sealed class MinioFileStorage : IFileStorage
	{
		private readonly IMinioClient _minio;
		public MinioFileStorage(IMinioClient minio) => _minio = minio;

		public async Task EnsureBucketAsync(string bucket, CancellationToken cancellationToken)
		{
			var exists = await _minio.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucket), cancellationToken);
			if (!exists)
			{
				await _minio.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucket), cancellationToken);
			}
		}

		public async Task UploadAsync(string bucket, string objectKey, Stream conent, string contentType, CancellationToken cancellationToken) {
			await _minio.PutObjectAsync(new PutObjectArgs()
				.WithBucket(bucket)
				.WithObject(objectKey)
				.WithStreamData(conent)
				.WithObjectSize(conent.Length)
				.WithContentType(contentType), cancellationToken
			);
		}

		public Task<string> GetPresignedGetUrlAsync(string bucket, string objectKey, TimeSpan expiry, CancellationToken cancellationToken)
			=> _minio.PresignedGetObjectAsync(
				new PresignedGetObjectArgs().WithBucket(bucket)
					.WithObject(objectKey)
					.WithExpiry((int)expiry.TotalSeconds)
				);
	}
}
