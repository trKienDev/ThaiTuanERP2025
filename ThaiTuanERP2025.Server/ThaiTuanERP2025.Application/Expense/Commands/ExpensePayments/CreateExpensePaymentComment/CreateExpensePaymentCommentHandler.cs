using AutoMapper;
using MediatR;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Expense.Dtos;
using ThaiTuanERP2025.Domain.Exceptions;
using ThaiTuanERP2025.Domain.Expense.Entities;

namespace ThaiTuanERP2025.Application.Expense.Commands.ExpensePayments.CreateExpensePaymentComment
{
	public sealed class CreateExpensePaymentCommentHandler : IRequestHandler<CreateExpensePaymentCommentCommand, ExpensePaymentCommentDto>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ICurrentUserService _currentUserService;
		public CreateExpensePaymentCommentHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_currentUserService = currentUserService;	
		}

		public async Task<ExpensePaymentCommentDto> Handle(CreateExpensePaymentCommentCommand command, CancellationToken cancellationToken)
		{
			var request = command.Request;
			var userId = _currentUserService.UserId ?? throw new NotFoundException("Cannot determine current user");
			var paymentExists = await _unitOfWork.ExpensePayments.SingleOrDefaultIncludingAsync(p => p.Id == request.ExpensePaymentId);

			ExpensePaymentComment? parent = null;
			if (request.ParentCommentId.HasValue)
			{
				parent = await _unitOfWork.ExpensePaymentComments.SingleOrDefaultIncludingAsync(c => c.Id == request.ParentCommentId.Value);
				if(parent is null) 
					throw new NotFoundException("Parent comment not found");
				if(parent.ExpensePaymentId != request.ExpensePaymentId)
					throw new ConflictException("Parent comment does not belong to the same expense payment");
				// không cho reply của reply
				var isReplyOfReply = await _unitOfWork.ExpensePaymentComments.AnyAsync(c => c.ParentCommentId == parent.Id);
				if (parent.ParentCommentId != null)
					throw new InvalidOperationException("Cannot reply to a child comment (one-level replies only).");
			}

			// Create comment
			var comment = new ExpensePaymentComment(request.ExpensePaymentId, request.Content, userId, request.ParentCommentId);

			// Thêm tags (nếu có)
			if(request.TaggedUserIds is not null && request.TaggedUserIds.Count > 0)
			{
				// khử trùng lặp
				var unique = request.TaggedUserIds.Where(id => id != Guid.Empty).Distinct().ToList();
				foreach (var uid in unique)
					comment.AddTag(uid, userId);
			}

			// Thêm attachments (nếu có)
			if (request.Attachments is not null && request.Attachments.Count > 0)
			{
				foreach (var att in request.Attachments)
				{
					comment.AddAttachment(att.FileName, att.FileUrl, att.FileSize, att.MimeType, att.FileId, userId);
				}
			}

			await _unitOfWork.ExpensePaymentComments.AddAsync(comment);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			var entity = await _unitOfWork.ExpensePaymentComments.SingleOrDefaultIncludingAsync(
				c => c.Id == comment.Id,
				true,
				cancellationToken,
				c => c.CreatedByUser,
				c => c.Attachments,
				c => c.Tags,
				c => c.Replies
			);

			var dto = _mapper.Map<ExpensePaymentCommentDto>(entity);
			return dto;
		}
	}
}
