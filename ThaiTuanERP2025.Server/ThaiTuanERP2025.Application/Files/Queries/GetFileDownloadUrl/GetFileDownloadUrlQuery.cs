using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThaiTuanERP2025.Application.Files.Queries.GetFileDownloadUrl
{
	public sealed record GetFileDownloadUrlQuery(Guid id) : IRequest<string>;
}
