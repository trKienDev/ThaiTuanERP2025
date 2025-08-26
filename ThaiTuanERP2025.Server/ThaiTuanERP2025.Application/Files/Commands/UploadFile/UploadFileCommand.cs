using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Files.Common;

namespace ThaiTuanERP2025.Application.Files.Commands.UploadFile
{
	public sealed record UploadFileCommand(RawFile File, string Module, string Entity, string? EntityId, bool IsPublic) : IRequest<UploadFileResult>;
	public sealed record UploadFileResult(Guid Id, string ObjectKey, long Size, string FileName, string ContentType);
}
