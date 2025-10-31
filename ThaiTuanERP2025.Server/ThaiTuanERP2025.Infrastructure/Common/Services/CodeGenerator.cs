using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Application.Common.Services;
using ThaiTuanERP2025.Domain.Common.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence;

namespace ThaiTuanERP2025.Infrastructure.Common.Services
{
	public class CodeGenerator : ICodeGenerator
	{
		private readonly ThaiTuanERP2025DbContext _dbContext;
		public CodeGenerator(ThaiTuanERP2025DbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<string> NextAsync(string key, string prefix, int padLength = 6, long start = 1, CancellationToken cancellationToken = default)
		{
			for (int attempt = 0; attempt < 5; attempt++)
			{
				// 1) Lấy series
				var series = await _dbContext.NumberSeries.SingleOrDefaultAsync(x => x.Key == key, cancellationToken);

				// 2) Nếu chưa có -> tạo mới
				if (series == null)
				{
					series = new NumberSeries(key, prefix, padLength, start);
					_dbContext.NumberSeries.Add(series);

					try
					{
						await _dbContext.SaveChangesAsync(cancellationToken);
					}
					catch (DbUpdateException)
					{
						_dbContext.ChangeTracker.Clear();
						continue;
					}
				}

				// 3) Cấp số tiếp theo qua behavior domain
				var code = series.GenerateNext();

				try
				{
					await _dbContext.SaveChangesAsync(cancellationToken);
					return code;
				}
				catch (DbUpdateConcurrencyException)
				{
					_dbContext.Entry(series).Reload();
					continue;
				}
			}

			throw new InvalidOperationException($"Không thể tạo mã mới cho {key} sau 5 lần thử. Vui lòng thử lại sau.");
		}

	}
}
