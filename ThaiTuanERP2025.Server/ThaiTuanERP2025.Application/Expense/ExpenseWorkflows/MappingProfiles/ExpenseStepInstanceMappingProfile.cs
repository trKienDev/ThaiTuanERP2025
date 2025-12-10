using AutoMapper;
using System.Text.Json;
using ThaiTuanERP2025.Application.Expense.ExpenseWorkflows.Contracts;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Shared.Utils;

namespace ThaiTuanERP2025.Application.Expense.ExpenseWorkflows.MappingProfiles
{
	public sealed class ExpenseStepInstanceMappingProfile : Profile
	{
		public ExpenseStepInstanceMappingProfile()
		{
			CreateMap<ExpenseStepInstance, ExpenseStepInstanceBriefDto>()
				.ForMember(dest => dest.ApprovedByUser, opt => opt.MapFrom(src => src.ApprovedByUser))
				.ForMember(dest => dest.DueAt, opt => opt.MapFrom(
					src => src.DueAt.HasValue ? TimeZoneConverter.ToVietnamTime(src.DueAt.Value) : (DateTime?)null
				)
			);

			CreateMap<ExpenseStepInstance, ExpenseStepInstanceDetailDto>()
				.ForMember(dest => dest.ApprovedByUser, opt => opt.MapFrom(src => src.ApprovedByUser))
				.ForMember(dest => dest.RejectedByUser, opt => opt.MapFrom(src => src.RejectedByUser))
				.ForMember(dest => dest.DueAt, opt => opt.MapFrom(
					src => src.DueAt.HasValue ? TimeZoneConverter.ToVietnamTime(src.DueAt.Value) : (DateTime?)null)
				)
				.AfterMap((src, dest) =>
				{
					dest.ApproverIds = string.IsNullOrEmpty(src.ResolvedApproversJson) 
						? new()
						: JsonSerializer.Deserialize<List<Guid>>(src.ResolvedApproversJson) ?? new();
				});
		}
	}
}
