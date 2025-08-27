using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Infrastructure.StoredFiles.Configurations;

namespace ThaiTuanERP2025.Infrastructure.StoredFiles.FileStorage
{
	public sealed class MinioFileStorage : IFileStorage, IFileStorageInfo
	{
		private readonly IMinioClient _minio;
		private readonly int _presignedExpirySeconds;
		public string BucketName { get; }
		public MinioFileStorage(IMinioClient minio, IOptions<FileStorageOptions> opt)
		{
			_minio = minio;
			BucketName = opt.Value.Bucket;
			_presignedExpirySeconds = opt.Value.PresignedExpirySeconds;
		}

		public async Task EnsureReadyAsync(CancellationToken cancellationToken)
		{
			var exists = await _minio.BucketExistsAsync(new BucketExistsArgs().WithBucket(BucketName), cancellationToken);
			if (!exists)
			{
				await _minio.MakeBucketAsync(new MakeBucketArgs().WithBucket(BucketName), cancellationToken);
			}
		}

		public async Task UploadAsync(string objectKey, Stream conent, string contentType, CancellationToken cancellationToken)
		{
			await _minio.PutObjectAsync(new PutObjectArgs()
				.WithBucket(BucketName)
				.WithObject(objectKey)
				.WithStreamData(conent)
				.WithObjectSize(conent.Length)
				.WithContentType(contentType), cancellationToken
			);
		}

		public Task<string> GetPresignedGetUrlAsync( string objectKey, CancellationToken cancellationToken)
			=> _minio.PresignedGetObjectAsync(
					new PresignedGetObjectArgs().WithBucket(BucketName)
						.WithObject(objectKey)
						.WithExpiry(_presignedExpirySeconds)
				);


		public Task RemoveAsync(string objectKey, CancellationToken cancellationToken)
		=> _minio.RemoveObjectAsync(
			new RemoveObjectArgs().WithBucket(BucketName).WithObject(objectKey), 
			cancellationToken
		);
	}
}
