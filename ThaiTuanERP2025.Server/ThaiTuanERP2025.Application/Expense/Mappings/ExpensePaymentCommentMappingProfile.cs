using AutoMapper;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Mappings
{
	public sealed class ExpensePaymentCommentMappingProfile : Profile
	{
		public ExpensePaymentCommentMappingProfile() {
			// Comment → CommentDto
			CreateMap<ExpensePaymentComment, ExpensePaymentCommentDto>()
				.ForMember(d => d.CommentType, o => o.MapFrom(s => (int)s.CommentType))
				.ForMember(d => d.CreatedByUser, o => o.MapFrom(s => s.CreatedByUser))
				.ForMember(d => d.Attachments, o => o.MapFrom(s => s.Attachments))
				.ForMember(d => d.Tags, o => o.MapFrom(s => s.Tags))
				.ForMember(d => d.Replies, o => o.MapFrom(s => s.Replies));

			// Tag → TagDto
			CreateMap<ExpensePaymentCommentTag, ExpensePaymentCommentTagDto>()
			    .ForCtorParam("UserId", o => o.MapFrom(s => s.UserId))
			    .ForCtorParam("FullName", o => o.MapFrom(s => s.User != null ? s.User.FullName : null)) // NULL-SAFE
			    .ForCtorParam("AvatarObjectKey", o => o.MapFrom(s => s.User != null ? s.User.AvatarFileObjectKey : null)); // NULL-SAFE

			// Attachment → AttachmentDto (ví dụ)
			CreateMap<ExpensePaymentCommentAttachment, ExpensePaymentCommentAttachmentDto>()
			    .ForCtorParam("Id", o => o.MapFrom(s => s.Id))
			    .ForCtorParam("FileName", o => o.MapFrom(s => s.FileName))
			    .ForCtorParam("FileUrl", o => o.MapFrom(s => s.FileUrl))
			    .ForCtorParam("FileSize", o => o.MapFrom(s => s.FileSize))
			    .ForCtorParam("MimeType", o => o.MapFrom(s => s.MimeType))
			    .ForCtorParam("FileId", o => o.MapFrom(s => s.FileId));

		}
	}
}
