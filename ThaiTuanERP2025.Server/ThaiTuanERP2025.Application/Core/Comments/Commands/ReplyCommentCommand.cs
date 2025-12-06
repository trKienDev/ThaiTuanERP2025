using FluentValidation;
using MediatR;
using ThaiTuanERP2025.Application.Core.Comments.Contracts;
using ThaiTuanERP2025.Application.Shared.Exceptions;
using ThaiTuanERP2025.Application.Shared.Interfaces;
using ThaiTuanERP2025.Domain.Core.Entities;
using ThaiTuanERP2025.Domain.Shared.Enums;
using ThaiTuanERP2025.Domain.Shared.Repositories;

namespace ThaiTuanERP2025.Application.Core.Comments.Commands
{
        public sealed record ReplyCommentCommand(Guid ParentId, CommentPayload Payload) : IRequest<CommentDetailDto>;
        public sealed class ReplyCommentCommandHandler : IRequestHandler<ReplyCommentCommand, CommentDetailDto>
        {
                private readonly IUnitOfWork _uow;
                private readonly ICommentReadRepository _commentRepo;
                private readonly ICurrentUserService _currentUser;
                public ReplyCommentCommandHandler(IUnitOfWork uow, ICommentReadRepository commentRepo, ICurrentUserService currentUser)
                {
                        _uow = uow;
                        _commentRepo = commentRepo;
                        _currentUser = currentUser;
                }

                public async Task<CommentDetailDto> Handle(ReplyCommentCommand command, CancellationToken cancellationToken)
                {
                        var payload = command.Payload;

                        var userId = _currentUser.UserId ?? throw new UnauthorizedException("User hiện tại không hợp lệ");        
                        
                        var documentType = Enum.Parse<DocumentType>(payload.DocumentType, ignoreCase: true);

                        var parentComment = await _uow.Comments.SingleOrDefaultAsync(
                                q => q.Where(x => 
                                        x.Id == command.ParentId
                                        && x.DocumentType == documentType
                                        && x.DocumentId == payload.DocumentId
                                ),
                                asNoTracking: false,
                                cancellationToken: cancellationToken
                        ) ?? throw new NotFoundException("Comment cha không tồn tại");

                        var reply = new Comment(
                                documentType,
                                payload.DocumentId,
                                userId,
                                payload.Content
                        );
                        await _uow.Comments.AddAsync(reply, cancellationToken);

                        parentComment.AddReply(reply);

                        await _uow.SaveChangesAsync(cancellationToken);
                        var replyDetail = await _commentRepo.GetDetailById(reply.Id, cancellationToken);
                        return replyDetail;
                }
        }

        public sealed class ReplyCommentCommandValidator : AbstractValidator<ReplyCommentCommand>
        {
                public ReplyCommentCommandValidator() {
                        RuleFor(x => x.ParentId).NotEmpty().WithMessage("Định danh comment cha không được bỏ trống");
                        RuleFor(x => x.Payload.Content).NotEmpty().WithMessage("Nội dung comment không được để trống");
                }       
        }
}
