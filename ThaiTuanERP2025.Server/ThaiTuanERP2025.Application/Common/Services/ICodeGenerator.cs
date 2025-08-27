using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Common.Services
{
	public interface ICodeGenerator
	{
		Task<string> NextAsync(string key, string prefix, int padLength = 6, long start = 1, CancellationToken cancellationToken = default);
	}
}
