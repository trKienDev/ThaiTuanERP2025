using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ThaiTuanERP2025.Application.Common.Options;
using ThaiTuanERP2025.Application.Common.Services;
using ThaiTuanERP2025.Domain.Common;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Common.Services
{
	public class DocumentSubIdGeneratorService : IDocumentSubIdGeneratorService
	{
		private readonly ThaiTuanERP2025DbContext _db;
		private readonly IOptions<DocumentSubIdOptions> _opt;
		public DocumentSubIdGeneratorService(ThaiTuanERP2025DbContext db, IOptions<DocumentSubIdOptions> opt)
		{
			_db = db;
			_opt = opt;
		}

		public async Task<string> NextSubIdAsync(string documentType, DateTime nowUtc, CancellationToken ct)
		{
			if (string.IsNullOrWhiteSpace(documentType))
				throw new ArgumentException("documentType is required", nameof(documentType));

			var (ddMMyyyy, yyyyMMdd) = GetVietnamDateStrings(nowUtc);
			var digit = ResolveDigit(documentType);

			// MỖI LOẠI CÓ COUNTER RIÊNG TRONG NGÀY:
			var key = $"{documentType}:{yyyyMMdd}";  // thay vì chỉ yyyyMMdd

			// LẤY EXECUTION STRATEGY VÀ CHẠY TOÀN BỘ NHƯ MỘT KHỐI CÓ THỂ RETRY
			var strategy = _db.Database.CreateExecutionStrategy();

			long nextNumber = 0;

			await strategy.ExecuteAsync(async () =>
			{
				await using var tx = await _db.Database.BeginTransactionAsync(ct);

				var seq = await _db.Set<DocumentSequence>()
				    .FromSqlRaw("SELECT * FROM DocumentSequences WITH (UPDLOCK, HOLDLOCK) WHERE [Key] = {0}", key)
				    .SingleOrDefaultAsync(ct);

				if (seq == null)
				{
					// khởi tạo record
					seq = new DocumentSequence { Key = key, LastNumber = 0 };
					_db.Add(seq);
					await _db.SaveChangesAsync(ct);

					// đọc lại có lock
					seq = await _db.Set<DocumentSequence>()
					    .FromSqlRaw("SELECT * FROM DocumentSequences WITH (UPDLOCK, HOLDLOCK) WHERE [Key] = {0}", key)
					    .SingleAsync(ct);
				}

				seq.LastNumber += 1;
				await _db.SaveChangesAsync(ct);

				nextNumber = seq.LastNumber;

				await tx.CommitAsync(ct);
			});

			var seqStr = nextNumber.ToString("D2"); // 01, 02, ... 10, 100 ...
			return $"{ddMMyyyy}{digit}{seqStr}";
		}


		private string ResolveDigit(string documentType)
		{
			if (_opt.Value.TypeDigits.TryGetValue(documentType, out var d) && !string.IsNullOrWhiteSpace(d))
				return d;
			throw new InvalidOperationException($"No digit mapping configured for DocumentType '{documentType}'.");
		}


		private static (string ddMMyyyy, string yyyyMMdd) GetVietnamDateStrings(DateTime nowUtc)
		{
			// Windows: "SE Asia Standard Time"
			// Linux/Docker: "Asia/Ho_Chi_Minh"
			TimeZoneInfo tz;
			try
			{
				tz = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
			}
			catch
			{
				tz = TimeZoneInfo.FindSystemTimeZoneById("Asia/Ho_Chi_Minh");
			}

			var nowVn = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, tz);
			var y = nowVn.Year; var m = nowVn.Month; var d = nowVn.Day;

			var ddMMyyyy = $"{d:D2}{m:D2}{y:D4}";
			var yyyyMMdd = $"{y:D4}{m:D2}{d:D2}";
			return (ddMMyyyy, yyyyMMdd);
		}
	}
}
