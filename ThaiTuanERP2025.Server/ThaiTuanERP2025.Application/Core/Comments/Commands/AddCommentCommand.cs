using AutoMapper;
using FluentValidation;
using MediatR;
using System.Xml.Linq;
using ThaiTuanERP2025.Application.Core.Comments.Contracts;
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
		public AddCommentCommandHandler(IUnitOfWork uow, ICurrentUserService currentUser, ICommentReadRepository commentRepo)
		{
			_uow = uow;	
			_currentUser = currentUser;
			_commentRepo = commentRepo;	
		}

		public async Task<CommentDetailDto> Handle(AddCommentCommand command, CancellationToken cancellationToken)
		{
			var payload = command.Payload;
			var userId = _currentUser.UserId ?? throw new UnauthorizedException("User không hợp lệ");

			var newComment = new Comment(
				payload.Module,
				payload.Entity,
				payload.EntityId,
				userId,
				payload.Content
			);

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
