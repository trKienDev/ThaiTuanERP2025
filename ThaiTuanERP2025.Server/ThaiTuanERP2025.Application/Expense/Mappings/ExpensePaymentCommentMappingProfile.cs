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
			    // map theo các tham số ctor của record ExpensePaymentCommentDto
			    .ForCtorParam("Id", o => o.MapFrom(s => s.Id))
			    .ForCtorParam("ExpensePaymentId", o => o.MapFrom(s => s.ExpensePaymentId))
			    .ForCtorParam("ParentCommentId", o => o.MapFrom(s => s.ParentCommentId))
			    .ForCtorParam("Content", o => o.MapFrom(s => s.Content))
			    .ForCtorParam("IsEdited", o => o.MapFrom(s => s.IsEdited))
			    .ForCtorParam("CommentType", o => o.MapFrom(s => (int)s.CommentType))
			    .ForCtorParam("CreatedByUserId", o => o.MapFrom(s => s.CreatedByUserId))
			    .ForCtorParam("CreatedByFullName", o => o.MapFrom(s => s.CreatedByUser != null ? s.CreatedByUser.FullName : "")) // NULL-SAFE
			    .ForCtorParam("CreatedByAvatar", o => o.MapFrom(s => s.CreatedByUser != null ? s.CreatedByUser.AvatarFileObjectKey : null)) // NULL-SAFE
			    .ForCtorParam("CreatedDate", o => o.MapFrom(s => s.CreatedDate))
			    .ForCtorParam("Attachments", o => o.MapFrom(s => s.Attachments))
			    .ForCtorParam("Tags", o => o.MapFrom(s => s.Tags))
			    .ForCtorParam("Replies", o => o.MapFrom(s => s.Replies));

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
