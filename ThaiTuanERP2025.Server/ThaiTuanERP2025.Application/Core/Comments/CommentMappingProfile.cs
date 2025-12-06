using AutoMapper;
using ThaiTuanERP2025.Application.Core.Comments.Contracts;
using ThaiTuanERP2025.Domain.Core.Entities;

namespace ThaiTuanERP2025.Application.Core.Comments
{
	public sealed class CommentMappingProfile : Profile 
	{
		public CommentMappingProfile() {
			CreateMap<Comment, CommentDto>();
		}
	}
}
