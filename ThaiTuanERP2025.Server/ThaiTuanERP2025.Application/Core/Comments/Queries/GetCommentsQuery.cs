using AutoMapper;
using FluentValidation;
using MediatR;
using ThaiTuanERP2025.Application.Core.Comments.Contracts;
using ThaiTuanERP2025.Domain.Shared.Enums;

namespace ThaiTuanERP2025.Application.Core.Comments.Queries
{
	public sealed record GetCommentsQuery(DocumentType DocumentType, Guid EntityId) : IRequest<List<CommentDetailDto>>;
	public sealed class GetCommentsQueryHandler : IRequestHandler<GetCommentsQuery, List<CommentDetailDto>>
	{
		private readonly ICommentReadRepository _commentRepo;
		private readonly IMapper _mapper;
		public GetCommentsQueryHandler(ICommentReadRepository commentRepo, IMapper mapper)
		{
			_commentRepo = commentRepo;
			_mapper = mapper;
		}

		public async Task<List<CommentDetailDto>> Handle(GetCommentsQuery query, CancellationToken cancellationToken)
		{
			var dtos = await _commentRepo.GetComments(query.DocumentType, query.EntityId, cancellationToken);
                        return BuildTree(dtos);
                }

                private List<CommentDetailDto> BuildTree(IReadOnlyList<CommentDetailDto> flat)
                {
                        var lookup = flat.ToDictionary(x => x.Id);
                        var roots = new List<CommentDetailDto>();

                        foreach (var c in flat)
                        {
                                if (c.ParentCommentId is null)
                                {
                                        roots.Add(c);
                                }
                                else
                                {
                                        if (lookup.TryGetValue(c.ParentCommentId.Value, out var parent))
                                        {
                                                parent.Replies.Add(c);
                                        }
                                }
                        }

                        return roots;
                }
        }

        public sealed class GetCommentsQueryValidator : AbstractValidator<GetCommentsQuery>
	{
		public GetCommentsQueryValidator() {
			RuleFor(x => x.EntityId).NotEmpty().WithMessage("Định danh của entity không được để trống");
		}	
	}
}
