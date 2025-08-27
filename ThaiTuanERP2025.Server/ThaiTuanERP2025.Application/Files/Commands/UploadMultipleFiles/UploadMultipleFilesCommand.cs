using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiTuanERP2025.Application.Files.Commands.UploadFile;
using ThaiTuanERP2025.Application.Files.Common;

namespace ThaiTuanERP2025.Application.Files.Commands.UploadMultipleFiles
{
	public sealed record UploadMultipleFilesCommand(List<RawFile> Files, string Module, string Entity, string? EntityId, bool IsPublic) : IRequest<List<UploadFileResult>>;
}
