using AutoMapper;
using ThaiTuanERP2025.Application.Core.Comments.Contracts;
using ThaiTuanERP2025.Domain.Core.Entities;

namespace ThaiTuanERP2025.Application.Core.Comments.MappingProfiles
{
	public sealed class CommentAttachmentMappingProfile : Profile
	{
		public CommentAttachmentMappingProfile() {
			CreateMap<CommentAttachment, CommentAttachmentDto>();
				//.ForMember(dest => dest.StoredFile, opt => opt.MapFrom(src => src.StoredFile));
		}	
	}
}
