using AutoMapper;
using ThaiTuanERP2025.Application.Core.FileAttachments.Contracts;
using ThaiTuanERP2025.Application.Core.FileAttachments.Repositories;
using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Infrastructure.Shared.Repositories;

namespace ThaiTuanERP2025.Infrastructure.Core.Repositories.Read
{
	public sealed class FileAttachmentReadRepository : BaseReadRepository<FileAttachment, FileAttachmentDto>, IFileAttachmentReadRepository
	{
		public FileAttachmentReadRepository(ThaiTuanERP2025DbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }
	}
}
