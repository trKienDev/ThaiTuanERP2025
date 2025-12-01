using AutoMapper;
using ThaiTuanERP2025.Application.Expense.ExpensePayments.Contracts;
using ThaiTuanERP2025.Domain.Expense.Entities;
using ThaiTuanERP2025.Domain.Shared.Utils;

namespace ThaiTuanERP2025.Application.Expense.ExpensePayments.MappingProfiles
{
	public sealed class ExpensePaymentMappingProfile : Profile { 
		public ExpensePaymentMappingProfile() {
			CreateMap<ExpensePayment, ExpensePaymentDto>();

			CreateMap<ExpensePayment, ExpensePaymentLookupDto>()
				//.ForMember(dest => dest.WorkflowInstance, opt => opt.MapFrom(src => src.CurrentWorkflowInstance))
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => TimeZoneConverter.ToVietnamTime(src.CreatedAt)));

			CreateMap<ExpensePayment, ExpensePaymentDetailDto>()
				.ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier != null ? src.Supplier.Name : null))
				.ForMember(dest => dest.WorkflowInstance, opt => opt.MapFrom(src => src.CurrentWorkflowInstance))
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => TimeZoneConverter.ToVietnamTime(src.CreatedAt)))
				.ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

			CreateMap<ExpensePaymentItem, ExpensePaymentItemLookupDto>()
				.ForMember(dest => dest.BudgetCode, opt => opt.MapFrom(src => src.BudgetPlanDetail.BudgetCode));
		}
	}
}
