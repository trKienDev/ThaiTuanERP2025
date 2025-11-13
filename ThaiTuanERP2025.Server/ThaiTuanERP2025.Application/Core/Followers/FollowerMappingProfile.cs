using AutoMapper;
using ThaiTuanERP2025.Domain.Core.Entities;

namespace ThaiTuanERP2025.Application.Core.Followers
{
	public sealed class FollowerMappingProfile : Profile
	{
		public FollowerMappingProfile() { 
			CreateMap<Follower, FolloweDto>();
		}
	}
}
