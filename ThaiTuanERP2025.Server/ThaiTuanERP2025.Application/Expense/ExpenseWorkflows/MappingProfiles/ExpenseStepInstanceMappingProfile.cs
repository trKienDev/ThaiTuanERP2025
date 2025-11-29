using AutoMapper;
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
                }
	}
}
