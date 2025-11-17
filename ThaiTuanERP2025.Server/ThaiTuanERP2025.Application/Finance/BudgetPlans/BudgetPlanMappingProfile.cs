using AutoMapper;
using ThaiTuanERP2025.Application.Finance.BudgetPlans.Contracts;
using ThaiTuanERP2025.Domain.Files.Events.StoredFiles;
using ThaiTuanERP2025.Domain.Finance.Entities;
using ThaiTuanERP2025.Domain.Shared.Utils;

namespace ThaiTuanERP2025.Application.Finance.BudgetPlans
{
	public class BudgetPlanMappingProfile : Profile
	{
		public BudgetPlanMappingProfile()
		{
			// ========== DETAIL DTO ==========
			CreateMap<BudgetPlanDetail, BudgetPlanDetailDto>()
				.ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
				.ForMember(d => d.BudgetCodeId, opt => opt.MapFrom(s => s.BudgetCodeId))
				.ForMember(d => d.BudgetCode, opt => opt.MapFrom(s => s.BudgetCode))
				.ForMember(d => d.Amount, opt => opt.MapFrom(s => s.Amount));

			// ========== PLAN DTO ==========
			CreateMap<BudgetPlan, BudgetPlanDto>()
				.ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
				.ForMember(d => d.DepartmentId, opt => opt.MapFrom(s => s.DepartmentId))
				.ForMember(d => d.Department, opt => opt.MapFrom(s => s.Department))
				.ForMember(d => d.BudgetPeriodId, opt => opt.MapFrom(s => s.BudgetPeriodId))
				.ForMember(d => d.BudgetPeriod, opt => opt.MapFrom(s => s.BudgetPeriod))
				.ForMember(d => d.TotalAmount, opt => opt.MapFrom(s => s.Details.Where(x => x.IsActive).Sum(x => x.Amount)))
				.ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status))
				.ForMember(d => d.CreatedAt, opt => opt.MapFrom(s => TimeZoneConverter.ToVietnamTime(s.CreatedAt)))
				.ForMember(d => d.DueAt, opt => opt.MapFrom(s => TimeZoneConverter.ToVietnamTime(s.DueAt)))

			    // Reviewer
			    // .ForMember(d => d.ReviewedByUserId, opt => opt.MapFrom(s => s.ReviewedByUserId))
			    // .ForMember(d => d.ReviewedByName, opt => opt.MapFrom(s => s.ReviewedByUser != null ? s.ReviewedByUser.FullName : null))
			    // .ForMember(d => d.ReviewedAt, opt => opt.MapFrom(s => s.ReviewedAt))

			    // Approver
			    // .ForMember(d => d.ApprovedByUserId, opt => opt.MapFrom(s => s.ApprovedByUserId))
			    // .ForMember(d => d.ApprovedByName, opt => opt.MapFrom(s => s.ApprovedByUser != null ? s.ApprovedByUser.FullName : null))
			    // .ForMember(d => d.ApprovedAt, opt => opt.MapFrom(s => s.ApprovedAt))

			    // Details
			    .ForMember(d => d.Details, opt => opt.MapFrom(s => s.Details.Where(x => x.IsActive)))

			    // Flags (phải set thủ công trong QueryHandler)
			    // .ForMember(d => d.CanEdit, opt => opt.Ignore())
			    .ForMember(d => d.CanReview, opt => opt.Ignore())
			    .ForMember(d => d.CanApprove, opt => opt.Ignore());
		}
	}
}
