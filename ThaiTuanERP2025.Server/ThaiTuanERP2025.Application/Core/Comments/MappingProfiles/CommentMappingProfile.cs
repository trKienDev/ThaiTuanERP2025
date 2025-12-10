using AutoMapper;
using ThaiTuanERP2025.Application.Core.Comments.Contracts;
using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Domain.Shared.Utils;

namespace ThaiTuanERP2025.Application.Core.Comments.MappingProfiles
{
	public sealed class CommentMappingProfile : Profile 
	{
		public CommentMappingProfile() {
			CreateMap<Comment, CommentDto>();

			CreateMap<Comment, CommentDetailDto>()
				.ForMember(dest => dest.User, src => src.MapFrom(x => x.User))
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => TimeZoneConverter.ToVietnamTime(src.CreatedAt)))
                                .ForMember(dest => dest.Replies, opt => opt.MapFrom(src => src.Replies))
				.ForMember(dest => dest.Mentions, opt => opt.MapFrom(src => src.Mentions));
                }
	}
}
