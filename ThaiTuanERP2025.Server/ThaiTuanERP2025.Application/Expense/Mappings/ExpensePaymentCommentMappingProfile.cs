using AutoMapper;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Mappings
{
	public sealed class ExpensePaymentCommentMappingProfile : Profile
	{
		public ExpensePaymentCommentMappingProfile() {
			CreateMap<ExpensePaymentCommentAttachment, ExpensePaymentCommentAttachmentDto>();

			CreateMap<ExpensePaymentCommentTag, ExpensePaymentCommentTagDto>()
			    .ForMember(d => d.FullName, cfg => cfg.MapFrom(s => s.User.FullName))
			    .ForMember(d => d.AvatarObjectKey, cfg => cfg.MapFrom(s => s.User.AvatarFileObjectKey));

			CreateMap<ExpensePaymentComment, ExpensePaymentCommentDto>()
				.ForMember(d => d.CommentType, cfg => cfg.MapFrom(s => (int)s.CommentType))
				.ForMember(d => d.CreatedByFullName, cfg => cfg.MapFrom(s => s.CreatedByUser!.FullName))
				.ForMember(d => d.CreatedByAvatar, cfg => cfg.MapFrom(s => s.CreatedByUser!.AvatarFileObjectKey))
				.ForMember(d => d.Attachments, cfg => cfg.MapFrom(s => s.Attachments))
				.ForMember(d => d.Tags, cfg => cfg.MapFrom(s => s.Tags))
				.ForMember(d => d.Replies, cfg => cfg.MapFrom(s => s.Replies));
		}
	}
}
