using AutoMapper;
using Drive.Application.Contracts;
using ThaiTuanERP2025.Domain.StoredFiles.Entities;

namespace Drive.Application
{
	public sealed class StoredFileMappingProfile : Profile
	{
		public StoredFileMappingProfile()
		{
			CreateMap<StoredObject, StoredFileMetadataDto>()
				.ForCtorParam("FileId", opt => opt.MapFrom(src => src.Id))
				.ForCtorParam("ObjectKey", opt => opt.MapFrom(src => src.ObjectKey))
				.ForCtorParam("FileName", opt => opt.MapFrom(src => src.FileName));
		}
	}
}
