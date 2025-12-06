using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using ThaiTuanERP2025.Application.Core.Comments.Contracts;

namespace ThaiTuanERP2025.Application.Core.Comments.Queries
{
	public sealed record GetCommentsQuery(string Module, string Entity, Guid EntityId) : IRequest<IReadOnlyList<CommentDto>>;
	public sealed class GetCommentsQueryHandler : IRequestHandler<GetCommentsQuery, IReadOnlyList<CommentDto>>
	{
		private readonly ICommentReadRepository _commentRepo;
		private readonly IMapper _mapper;
		public GetCommentsQueryHandler(ICommentReadRepository commentRepo, IMapper mapper)
		{
			_commentRepo = commentRepo;
			_mapper = mapper;
		}

		public async Task<IReadOnlyList<CommentDto>> Handle(GetCommentsQuery query, CancellationToken cancellationToken)
		{
			return await _commentRepo.ListProjectedAsync(
				q => q.Where(x =>
					x.Entity == query.Entity &&
					x.Module == query.Module &&
					x.EntityId == query.EntityId
				).ProjectTo<CommentDto>(_mapper.ConfigurationProvider),
				cancellationToken: cancellationToken
			);
		}
	}

	public sealed class GetCommentsQueryValidator : AbstractValidator<GetCommentsQuery>
	{
		public GetCommentsQueryValidator() {
			RuleFor(x => x.Module).NotEmpty().WithMessage("Định danh module của comment không được để trống");
			RuleFor(x => x.Entity).NotEmpty().WithMessage("Định danh entity của comment không được để trống");
			RuleFor(x => x.EntityId).NotEmpty().WithMessage("Định danh của entity không được để trống");
		}	
	}
}
