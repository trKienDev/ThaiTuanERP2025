using AutoMapper;
using ThaiTuanERP2025.Application.Files.Contracts;
using ThaiTuanERP2025.Domain.StoredFiles.Entities;

namespace ThaiTuanERP2025.Application.Files
{
	public sealed class StoredFileMappingProfile : Profile
	{
		public StoredFileMappingProfile() {
			CreateMap<StoredFile, StoredFileMetadataDto>()
				.ForCtorParam("FileId", opt => opt.MapFrom(src => src.Id))
				.ForCtorParam("ObjectKey", opt => opt.MapFrom(src => src.ObjectKey))
				.ForCtorParam("FileName", opt => opt.MapFrom(src => src.FileName));
		}
	}
}
