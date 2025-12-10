using AutoMapper;
using ThaiTuanERP2025.Application.Core.Comments.Contracts;
using ThaiTuanERP2025.Domain.Core.Entities;

namespace ThaiTuanERP2025.Application.Core.Comments.MappingProfiles
{
	public sealed class CommentMentionMappingProfile : Profile
	{
		public CommentMentionMappingProfile()
		{
			CreateMap<CommentMention, CommentMentionDto>()
				.ForMember(d => d.FullName, o => o.MapFrom(src => src.User.FullName));
		}
	}
}
