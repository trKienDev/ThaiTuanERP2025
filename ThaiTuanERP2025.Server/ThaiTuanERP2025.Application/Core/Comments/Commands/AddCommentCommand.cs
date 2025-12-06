using AutoMapper;
using FluentValidation;
using MediatR;
using System.Xml.Linq;
using ThaiTuanERP2025.Application.Account.Users.Repositories;
using ThaiTuanERP2025.Application.Core.Comments.Contracts;
using ThaiTuanERP2025.Application.Core.Followers;
using ThaiTuanERP2025.Application.Core.Notifications;
using ThaiTuanERP2025.Application.Expense.ExpensePayments.Repositories;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Application.Shared.Interfaces;
using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Core.Comments.Commands
{
	public sealed record AddCommentCommand(CommentPayload Payload) : IRequest<CommentDetailDto>;

	public sealed class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, CommentDetailDto>
	{
		private readonly IUnitOfWork _uow;
		private readonly ICurrentUserService _currentUser;
		private readonly ICommentReadRepository _commentRepo;
		private readonly INotificationService _notificationService;
		private readonly IExpensePaymentReadRepository _expensePaymentRepo;
		private readonly IFollowerReadRepository _followerRepo;
		private readonly IUserReadRepostiory _userRepo;
		public AddCommentCommandHandler(
			IUnitOfWork uow, ICurrentUserService currentUser, ICommentReadRepository commentRepo, INotificationService notificationService, IExpensePaymentReadRepository expensePaymentRepo,
			IFollowerReadRepository followerRepo, IUserReadRepostiory userRepo
		)
		{
			_uow = uow;	
			_currentUser = currentUser;
			_commentRepo = commentRepo;
			_notificationService = notificationService;
			_expensePaymentRepo = expensePaymentRepo;
			_followerRepo = followerRepo;
			_userRepo = userRepo;
		}

		public async Task<CommentDetailDto> Handle(AddCommentCommand command, CancellationToken cancellationToken)
		{
			var payload = command.Payload;

			var userId = _currentUser.UserId ?? throw new UnauthorizedException("User không hợp lệ");
			var userName = await _userRepo.GetUserNameAsync(userId, cancellationToken);

			var newComment = new Comment(
				payload.Module,
				payload.Entity,
				payload.EntityId,
				userId,
				payload.Content
			);

                        #region Send notifications to followers
			switch(payload.Entity)
			{
				case "expense-payment":
					var followerIds = await _followerRepo.GetFollowerIdsByDocument(payload.EntityId, Domain.Shared.Enums.DocumentType.ExpensePayment, cancellationToken);
                                        followerIds = followerIds.Where(id => id != userId).ToList();
                                        var expensePaymentName = await _expensePaymentRepo.GetNameAsync(payload.EntityId, cancellationToken);
					await _notificationService.SendToManyAsync(
						senderId: userId,
						userIds: followerIds,
						title: $"{expensePaymentName} có bình luận mới",
						message: $"{userName} đã bình luận {expensePaymentName}",
						linkType: Domain.Core.Enums.LinkType.ExpensePaymentDetail,
						targetId: payload.EntityId,
						type: Domain.Core.Enums.NotificationType.Info,
						cancellationToken: cancellationToken
					);
					break;
				default:
					break;
			}
                        #endregion

                        await _uow.Comments.AddAsync( newComment, cancellationToken);
			await _uow.SaveChangesAsync(cancellationToken);

			var commentDetail = await _commentRepo.GetDetailById(newComment.Id, cancellationToken);
			return commentDetail;
		}
	}

	public sealed class AddCommentCommandValidator : AbstractValidator<AddCommentCommand>
	{
		public AddCommentCommandValidator() {
			RuleFor(x => x.Payload.Module)
				.NotEmpty().WithMessage("Định danh module của comment không được để trống")
				.MaximumLength(64).WithMessage("Định danh module của comment không vượt quá 64 ký tự");
			RuleFor(x => x.Payload.Entity)
				.NotEmpty().WithMessage("Định danh entity của comment không được để trống")
				.MaximumLength(128).WithMessage("Định danh entity của comment không vượt quá 128 ký tự");
			RuleFor(x => x.Payload.EntityId).NotEmpty().WithMessage("Định danh của entity không được để trống");
			RuleFor(x => x.Payload.Content).NotEmpty().WithMessage("Nội dung của comment không được để trống");
		}	
	}
}
