using AutoMapper;
using ThaiTuanERP2025.Domain.Core.Entities;

namespace ThaiTuanERP2025.Application.Core.OutboxMessages
{
	public sealed class OutboxMessageMappingProfile : Profile
	{
		public OutboxMessageMappingProfile() { 
			CreateMap<OutboxMessage, OutboxMessageDto>();
		}
	}
}
