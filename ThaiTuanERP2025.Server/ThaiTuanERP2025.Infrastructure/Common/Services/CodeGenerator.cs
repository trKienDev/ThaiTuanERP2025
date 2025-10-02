using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Common.Services;
using ThaiTuanERP2025.Domain.Common;
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
			// tối đa 5 lần retry để xử lý cạnh tranh
			for (int attemp = 0; attemp < 5; attemp++)
			{
				// 1) Lấy series
				var series = await _dbContext.NumberSeries.SingleOrDefaultAsync(x => x.Key == key, cancellationToken);

				// 2) Nếu chưa có -> tạo mới theo tham số (không hardcode)
				if (series == null)
				{
					series = new NumberSeries
					{
						Key = key,
						Prefix = prefix,
						PadLength = padLength,
						NextNumber = start
					};
					_dbContext.NumberSeries.Add(series);

					try
					{
						await _dbContext.SaveChangesAsync(cancellationToken);
					}
					catch (DbUpdateException)
					{
						_dbContext.ChangeTracker.Clear(); // Xóa cache để tránh lỗi lặp lại
						continue; // Thử lại nếu có lỗi cập nhật
					}
				}

				// 3) Cấp số tiếp theo
				var number = series.NextNumber;
				series.NextNumber = number + 1;

				try
				{
					await _dbContext.SaveChangesAsync(cancellationToken);
					return $"{series.Prefix}{number.ToString().PadLeft(series.PadLength, '0')}";
				}
				catch (DbUpdateConcurrencyException)
				{
					// Có request khác vừa update series -> reload và thử lại
					_dbContext.Entry(series).Reload();
					continue;
				}
			}

			throw new InvalidOperationException($"Không thể tạo mã mới cho {key} sau 5 lần thử. Vui lòng thử lại sau.");

		}


	}
}
