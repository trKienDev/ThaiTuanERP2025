using AutoMapper;
using ThaiTuanERP2025.Application.Core.Files.Contracts;
using ThaiTuanERP2025.Domain.Core.Entities;

namespace ThaiTuanERP2025.Application.Core.Files
{
	public sealed class StoredFileMappingProfile : Profile
	{
		public StoredFileMappingProfile() {
			CreateMap<StoredFile, StoredFileMetadataDto>()
				.ForCtorParam("FileId", opt => opt.MapFrom(src => src.Id))
				.ForCtorParam("ObjectKey", opt => opt.MapFrom(src => src.ObjectKey))
				.ForCtorParam("FileName", opt => opt.MapFrom(src => src.FileName))
				.ForCtorParam("IsPublic", opt => opt.MapFrom(src => src.IsPublic));
		}
	}
}
