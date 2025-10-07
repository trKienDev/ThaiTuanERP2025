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
				.ForMember(d => d.CreatedByUsername, opt => opt.MapFrom(s => s.CreatedByUser.Username))
				
				// Collections
				.ForMember(d => d.Items, opt => opt.MapFrom(s => s.Items))
				.ForMember(d => d.Attachments, opt => opt.MapFrom(s => s.Attachments))
				.ForMember(d => d.Followers, opt => opt.MapFrom(s => s.Followers))
				// Supplier
				.ForMember(d => d.Supplier, opt => opt.MapFrom(s => s.Supplier))
				// Enum -> int (AutoMapper tự chuyển, dòng này có thể bỏ)
				.ForMember(d => d.Status, opt => opt.MapFrom(s => (int)s.Status))
				// Invoices nếu bạn có navigation từ Items -> Invoice
				.ForMember(d => d.Invoices, opt => opt.MapFrom(s => s.Items.Where(i => i.Invoice != null).Select(i => i.Invoice).Distinct()));

			// ===== Sub-entities =====
			CreateMap<ExpensePaymentItem, ExpensePaymentItemDto>();
			CreateMap<ExpensePaymentAttachment, ExpensePaymentAttachmentDto>();
			CreateMap<ExpensePaymentFollower, ExpensePaymentFollowersDto>();
		}
	}
}
