using AutoMapper;
using ThaiTuanERP2025.Application.Core.FileAttachments.Contracts;
using ThaiTuanERP2025.Domain.Core.Entities;

namespace ThaiTuanERP2025.Application.Core.FileAttachments.MappingProfiles
{
	public sealed class FileAttachmentMappingProfile : Profile
	{
		public FileAttachmentMappingProfile() {
			CreateMap<FileAttachment, FileAttachmentDto>();
		}
	}
}
