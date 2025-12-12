using ThaiTuanERP2025.Application.Core.FileAttachments.Contracts;
using ThaiTuanERP2025.Application.Shared.Repositories;
using ThaiTuanERP2025.Domain.Core.Entities;

namespace ThaiTuanERP2025.Application.Core.FileAttachments.Repositories
{
	public interface IFileAttachmentReadRepository : IBaseReadRepository<FileAttachment, FileAttachmentDto>
	{
	}
}
