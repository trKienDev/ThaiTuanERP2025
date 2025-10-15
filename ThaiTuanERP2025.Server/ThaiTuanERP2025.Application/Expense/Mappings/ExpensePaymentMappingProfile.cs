using AutoMapper;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Mappings
{
	public sealed class ExpensePaymentMappingProfile : Profile
	{
		public ExpensePaymentMappingProfile()
		{
			CreateMap<ExpensePayment, ExpensePaymentDto>();

			CreateMap<ExpensePayment, ExpensePaymentDetailDto>()
				// Creator
				.ForMember(d => d.CreatedByUserId, opt => opt.MapFrom(s => s.CreatedByUserId))
				.ForMember(d => d.CreatedByUser, opt => opt.MapFrom(s => s.CreatedByUser))
				// Collections
				.ForMember(d => d.Items, opt => opt.MapFrom(s => s.Items))
				.ForMember(d => d.Attachments, opt => opt.MapFrom(s => s.Attachments))
				.ForMember(d => d.Followers, opt => opt.MapFrom(s => s.Followers))
				// Supplier
				.ForMember(d => d.Supplier, opt => opt.MapFrom(s => s.Supplier))
				// Enum -> int (AutoMapper tự chuyển, dòng này có thể bỏ)
				.ForMember(d => d.Status, opt => opt.MapFrom(s => (int)s.Status));
			// Invoices nếu bạn có navigation từ Items -> Invoice

			CreateMap<ExpensePayment, ExpensePaymentSummaryDto>()
				.ForMember(d => d.Status, opt => opt.MapFrom(s => (int)s.Status))
				.ForMember(d => d.WorkflowInstanceStatus, o => o.MapFrom(s => s.CurrentWorkflowInstance));

			// ===== Sub-entities =====
			CreateMap<ExpensePaymentItem, ExpensePaymentItemDto>();
			CreateMap<ExpensePaymentItem, ExpensePaymentItemDetailDto>()
				.IncludeBase<ExpensePaymentItem, ExpensePaymentItemDto>() // tận dụng mapping base
				.ForMember(d => d.BudgetCode, o => o.MapFrom(s => s.BudgetCode))
				.ForMember(d => d.CashoutCode, o => o.MapFrom(s => s.CashoutCode))
				.ForMember(d => d.Invoice, o => o.MapFrom(s => s.Invoice));

			CreateMap<ExpensePaymentAttachment, ExpensePaymentAttachmentDto>();

			CreateMap<ExpensePaymentFollower, ExpensePaymentFollowersDto>();
		}
	}
}
